import sys
import os
import argparse
import logging
import shutil
import platform
import pkg_resources
import stat

from ament_index_python import get_package_prefix, get_package_share_directory
from ament_index_python.packages import PackageNotFoundError


class UnityROS2LibCopier:
    def __init__(self, unity_project_path):
        self.ament_dependencies = [
            'rcl',
            'rcl_interfaces',
            'rmw',
            'rmw_implementation',
            'rmw_fastrtps_cpp',
            'rmw_fastrtps_dynamic_cpp',
            'rmw_fastrtps_shared_cpp',
            'rosidl_generator_c',
            'rosidl_typesupport_c',
            'rosidl_typesupport_fastrtps_c',
            'rosidl_typesupport_fastrtps_cpp',
            'rcl_logging_noop',
            'builtin_interfaces',
            'rcutils',
            'rcldotnet',
            'std_msgs',
            'geometry_msgs',
            'sensor_msgs',
            'tf2_msgs',
            'nav_msgs',
            'test_msgs',
            'action_msgs',
            'unique_identifier_msgs',
        ]

        self.dependencies = [
            'poco_vendor',
            'fastcdr',
            'fastrtps',
        ]

        self.c_lib_source_dict = {}
        if platform.system() == 'Windows':
            self.c_lib_destination_dir = unity_project_path + '/Assets/Plugins/Windows/x86_64'
        else:
            self.c_lib_destination_dir = unity_project_path + '/Assets/Plugins/Linux/x86_64'

        self.cs_lib_source_dict = {}
        self.cs_lib_destination_dir = unity_project_path + '/Assets/Plugins'

        self.__find_libs()

    def copy_files(self):
        self.__copy_files_from_dict(self.c_lib_source_dict,
                                    self.c_lib_destination_dir)

        self.__copy_files_from_dict(self.cs_lib_source_dict,
                                    self.cs_lib_destination_dir)

    def __find_libs(self):
        self.__find_c_libs()
        self.__find_csharp_libs()

    def __find_c_libs(self):
        for package_name in self.ament_dependencies:
            try:
                package_install_path = get_package_prefix(package_name)
                package_install_parent_directory = os.path.abspath(
                    os.path.join(package_install_path, os.pardir))

                if platform.system() == 'Windows':
                    filename_extensions = ('.dll')
                    lib_folder = '\\bin'
                    package_lib_path = package_install_path + lib_folder
                else:
                    filename_extensions = ('.so', '.so.1', 'so.2')
                    lib_folder = '/lib'
                    package_lib_path = package_install_path + lib_folder

                if os.path.isdir(package_lib_path):
                    for c_lib_file in os.listdir(package_lib_path):
                        if c_lib_file.endswith(filename_extensions):
                            self.c_lib_source_dict[c_lib_file] = (
                                package_lib_path + '/' + c_lib_file)

                # Find ament compiled projects not found by ament_index
                # by looking in parent folders of ros install directories
                for non_ros_package in self.dependencies:
                    if non_ros_package in os.listdir(package_install_parent_directory):
                        lib_path = package_install_parent_directory + '/' + non_ros_package + lib_folder
                        if os.path.isdir(lib_path):
                            try:
                                for c_lib_file in os.listdir(lib_path):
                                    if c_lib_file.endswith(filename_extensions):
                                        self.c_lib_source_dict[c_lib_file] = (
                                            lib_path + '/' + c_lib_file)
                            except PackageNotFoundError:
                                pass

            except PackageNotFoundError:
                print('{} not found!'.format(package_name))

    def __find_csharp_libs(self):
        for package_name in self.ament_dependencies:
            try:
                package_install_path = get_package_prefix(package_name)
                package_lib_path = package_install_path + '/lib/dotnet'
                if os.path.isdir(package_lib_path):
                    for csharp_lib_file in os.listdir(package_lib_path):
                        if csharp_lib_file.endswith('.dll'):
                            self.cs_lib_source_dict[csharp_lib_file] = (
                                package_lib_path + '/' + csharp_lib_file)
            except PackageNotFoundError:
                print('{} not found!'.format(package_name))

    def __copy_files_from_dict(self, source_dictionary, destination_directory):
        if not os.path.isdir(destination_directory):
            try:
                os.makedirs(destination_directory)
            except OSError:
                logging.error('Failed to create directory {}'.format(
                    destination_directory))
            else:
                logging.info('Successfully created the directory {}'.format(
                    destination_directory))

        for filename, source_path in source_dictionary.items():
            destination_path = destination_directory + '/' + filename
            if should_copy_file(source_path, destination_path):
                logging.info('Copying "{}"'.format(filename))
                shutil.copy2(source_path, destination_path)


def should_copy_file(source_path, destination_path):

    def does_not_exist(destination_path):
        return not os.path.isfile(destination_path)

    def newer_verson_available(source_path, destination_path):
        return (os.stat(source_path).st_mtime
                - os.stat(destination_path).st_mtime > 0)

    return (does_not_exist(destination_path)
            or newer_verson_available(source_path, destination_path))


def parse_args(args):
    parser = argparse.ArgumentParser()

    parser.add_argument(
        '--unity-project', default=os.getcwd(), help='path to Unity project'
    )

    return parser.parse_args(args)


def directory_is_a_unity_project(dir_path):
    unity_project_settings = dir_path + '/ProjectSettings/ProjectSettings.asset'
    return os.path.isfile(unity_project_settings)


def copy_unity_files(unity_project_path):
    # unity_files_path = pkg_resources.resource_filename('rcldotnet_utils',
    #                                                    'unity_files')
    unity_files_path = get_package_share_directory('rcldotnet_utils') + '/unity_files'

    asset_path = unity_project_path + '/Assets'

    unity_resource_path = asset_path + '/Resources'
    if not os.path.isdir(unity_resource_path):
        try:
            os.makedirs(unity_resource_path)
        except OSError:
            logging.error('Failed to create directory {}'.format(
                unity_resource_path))
        else:
            logging.info('Successfully created the directory {}'.format(
                unity_resource_path))

    unity_editor_scripts_path = asset_path + '/Editor/ROS'
    if not os.path.isdir(unity_editor_scripts_path):
        try:
            os.makedirs(unity_editor_scripts_path)
        except OSError:
            logging.error('Failed to create directory {}'.format(
                unity_editor_scripts_path))
        else:
            logging.info('Successfully created the directory {}'.format(
                unity_editor_scripts_path))

    source_path = unity_files_path + '/start_editor.py'
    destination_path = asset_path + '/start_editor.py'
    if should_copy_file(source_path, destination_path):
        logging.info('Copying start_editor.py')
        shutil.copy2(source_path, destination_path)

    if platform.system() == 'Windows':
        source_path = unity_files_path + '/start_editor.bat'
        destination_path = asset_path + '/start_editor.bat'
    else:
        source_path = unity_files_path + '/start_editor.bash'
        destination_path = asset_path + '/start_editor.bash'
    if should_copy_file(source_path, destination_path):
        logging.info('Copying start_editor script')
        shutil.copy2(source_path, destination_path)

    if platform.system() == 'Linux':
        logging.info('Making editor script executable')
        st = os.stat(destination_path)
        os.chmod(destination_path, st.st_mode | stat.S_IEXEC)

    source_path = unity_files_path + '/start_player.py'
    destination_path = unity_resource_path + '/start_player.py'
    if should_copy_file(source_path, destination_path):
        logging.info('Copying start_player.py')
        shutil.copy2(source_path, destination_path)

    if platform.system() == 'Windows':
        source_path = unity_files_path + '/start_player.bat'
        destination_path = unity_resource_path + '/start_player.bat'
    else:
        source_path = unity_files_path + '/start_player.bash'
        destination_path = unity_resource_path + '/start_player.bash'
    if should_copy_file(source_path, destination_path):
        logging.info('Copying start_player script')
        shutil.copy2(source_path, destination_path)

    if platform.system() == 'Linux':
        logging.info('Making player script executable')
        st = os.stat(destination_path)
        os.chmod(destination_path, st.st_mode | stat.S_IEXEC)

    source_path = unity_files_path + '/BuildRos.cs'
    destination_path = unity_editor_scripts_path + '/BuildRos.cs'
    if should_copy_file(source_path, destination_path):
        logging.info('Copying BuildRos.cs')
        shutil.copy2(source_path, destination_path)


def main():
    logging.basicConfig(level=logging.DEBUG)

    parsed = parse_args(sys.argv[1:])

    if directory_is_a_unity_project(parsed.unity_project):
        lib_copier = UnityROS2LibCopier(parsed.unity_project)
        lib_copier.copy_files()

        copy_unity_files(parsed.unity_project)
    else:
        logging.error('The selected folder "{}" does not appear to be the root of a Unity3D project.'.format(
            parsed.unity_project
        ))


if __name__ == '__main__':
    main()
