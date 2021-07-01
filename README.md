# Look-into-memory

## Inspiration
The inspiration came from the CE and Reclass softwares.
These softwares are really complex and it interests me if I can even learn how to do it.
Today after half a year I share with you the first version of Lim Engine (currently SmScanner)

## The goal
The goal is to build software that can control every process that runs on your computer.
After an in-depth investigation on the subject I decided these were my next steps
+ c   : kernel driver that will provide me with a clean api to mess with memory
+ c++ :dll who can communicate with the driver
+ c++ :disassembler
+ c#  :disassembler wrapper for 32 bit process
+ c#  :gui

## How it work
The software provides the user with a convenient and clear interface with a variety of options
The software has 4 main windows
+ Scanner     : user can scan values in the process virtual memory
+ Memory      : user can navigate to different memory areas and see disassembly code or hex values
+ Structs     : user can scan a memory area and read it at whatever values are convenient for him
+ Information : details of the selected process
Each window has its own features.

#### System Requirements

+ x64 Windows 10;
+ Administrative privilege is required.

## Smdkd - Kernel Driver
The driver is unsigned and unapproved is Your responsibility. !!
To use the driver you need to either install manually or download and use KDU to load it into memory
look at https://github.com/hfiref0x/KDU Kernel Driver Utility


