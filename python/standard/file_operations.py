﻿# ===================================================
#                     DESCRIPTION
# ===================================================

'''
Python program to handle file operations
'''

# ===================================================
#                       IMPORTS
# ===================================================

import os
import shutil

# ===================================================
#                     DETECT FILE
# ===================================================


def detect_file(path):
    if os.path.exists(path):
        print("This location exists!")
        if os.path.isfile(path):
            print("This is a file.")
        elif os.path.isdir(path):
            print("This is a directory.")
    else:
        print("This locations doesn't exist!")

# ===================================================
#                       READ FILE
# ===================================================


def read_file(path):
    try:
        with open(path) as file:
            print(file.read())
    except FileNotFoundError:
        print("This file was not found.")

# ===================================================
#                       WRITE FILE
# ===================================================


def write_file(path):
    text = input("Insert text into the file: ")
    with open(path, 'w') as file:
        file.write(f'{text}\n')

# ===================================================
#                       COPY FILE
# ===================================================

# copyfile() =  copies contents of a file
# copy() =      copyfile() + permission mode + destination can be a directory
# copy2() =     copy() + copies metadata (file’s creation and modification times)


def copy_file(curr_path):
    destination = input("Enter a destination file: ")
    shutil.copyfile(curr_path, destination)

# ===================================================
#                       MOVE FILE
# ===================================================


def move_file(source):
    try:
        destination = input("Enter the destination directory: ")
        if os.path.exists(destination):
            print(f"There is alreaddy a file on the {destination} path.")
        else:
            os.replace(source, destination)
            print(f"{source} was moved.")
    except FileNotFoundError:
        print(f"{path} was not found.")

# ===================================================
#                       DELETE FILE
# ===================================================


def delete_file(path):
    try:
        if os.path.isfile(path):
            os.remove(path)
        elif os.path.isdir(path):
            shutil.rmtree(path)
    except FileNotFoundError:
        print(f"{path} was not found.")
    except PermissionError:
        print(f"You do not have a permission to delete {path}.")
    except OSError:
        print(f"You cannot delete {path} using os.path.rmdir(path) function.")
    else:
        print(f"{path} was deleted.")

# ===================================================
#                     MAIN FUNCTION
# ===================================================


path = input("Provide a path to the file: ")

# detect_file(path)
# read_file(path)
# write_file(path)
# copy_file(path)
# move_file(path)
delete_file(path)
