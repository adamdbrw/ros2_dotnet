from setuptools import find_packages
from setuptools import setup
package_name = 'rcldotnet_utils'
setup(
    name=package_name,
    version='0.0.0',
    packages=find_packages(exclude=['test']),
    data_files=[
        ('share/ament_index/resource_index/packages',
            ['resource/' + package_name]),
        ('share/' + package_name, ['package.xml']),
    ],
    install_requires=['setuptools'],
    zip_safe=True,
    author='Samuel Lindgren',
    author_email='samuel@dynorobotics.se',
    maintainer='Samuel Lindgren',
    maintainer_email='samuel@dynorobotics.se',
    keywords=['ROS'],
    classifiers=[
        'Intended Audience :: Developers',
        'License :: OSI Approved :: Apache License 2.0',
        'Programming Language :: Python',
    ],
    description=(
        'Utility scripts for rlcdotnet'
    ),
    license='Apache Licence 2.0',
    tests_require=['pytest'],
    entry_points={
        'console_scripts': [
            'init_unity_project = rcldotnet_utils.init_unity_project:main'
        ],
    },
)
