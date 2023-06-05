# ===================================================
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
#                     MAIN FUNCTION
# ===================================================


path = input("Provide a path to the file: ")

# detect_file(path)
# read_file(path)
# write_file(path)
copy_file(path)
