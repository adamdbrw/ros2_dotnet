ROS2 for .NET
=============

Since the project was last updated, I worked on a closed source version at Robotec.AI and made plenty of improvements. Please contact me (adam.dabrowski@robotec.ai) if you would like to use the module. 

[![CircleCI](https://circleci.com/gh/samiamlabs/ros2_dotnet/tree/dashing.svg?style=svg)](https://circleci.com/gh/samiamlabs/ros2_dotnet/tree/master)

ROS2 C# client library implementation. To my knowledge the most advanced ros2 C# project.

Notice
------

This fork started with https://github.com/esteve/ros2_dotnet/ implementation of rclcs with and integrated improvements from https://github.com/DynoRobotics/unity_ros2. Since then, plenty of features and fixes have been added, including:
*  Target version of ROS2 is now Dashing. This includes significant changes in message generation introduced between Crystal and Dashing
*  Added support for Nested types, Namespaced types and Sequences (basically you can use almost all standard messages).
*  Support for Header abstraction (MessageWithHeader interface), allowing polymorphic handling of generated messages.
*  Multiple other bug fixes and improvements in the library.
*  Resolved issues with the Unity libraries loading.

Since the fork contains rather revolutionary, large changes and includes a body of third person's work, it would need plenty of effort to integrate with the ros2_dotnet main.

Linux
-----

Make sure to source your ROS2 Dashing environment

```
mkdir -p ~/ros2_dotnet_ws/src
cd ~/ros2_dotnet_ws
wget https://raw.githubusercontent.com/samiamlabs/ros2_dotnet/master/ros2_dotnet_dashing.repos
vcs import ~/ros2_dotnet_ws/src < ros2_dotnet_dashing.repos
colcon build

```
Running. Source install/setup.bash in both terminals:

Talker:
```
ros2 run rcldotnet_examples rcldotnet_talker
```
Listener
```
ros2 run rcldotnet_examples rcldotnet_listener
```

Testing. Source install/setup.bash in terminal and change directory to rcldotnet_tests under src:

```
dotnet watch test
```

Contribution
------

See Projects page for what tasks and contributions are needed.
