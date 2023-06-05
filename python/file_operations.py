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
#                     MAIN FUNCTION
# ===================================================


path = input("Provide a path to the file: ")

detect_file(path)
