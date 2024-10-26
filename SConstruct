import platform

print("Syldeyn - SCons Script!")
print("(c) Jeroen P. Broks")
print("Platform: %s\n"%platform.system())


SyldeynFiles = [
    "../../Libs/JCR6/Source/JCR6_Core.cpp",
    "../../Libs/JCR6/Source/JCR6_Write.cpp",
#   "../../Libs/JCR6/Source/JCR6_zlib.cpp", # zlib for the time being deactivated! Must be present in Windows, not on other platforms (yet).
    "../../Libs/Units/Source/SlyvAsk.cpp",
    "../../Libs/Units/Source/SlyvBank.cpp",
    "../../Libs/Units/Source/SlyvDir.cpp",
    "../../Libs/Units/Source/SlyvQCol.cpp",
    "../../Libs/Units/Source/SlyvRoman.cpp",
    "../../Libs/Units/Source/SlyvSTOI.cpp",
    "../../Libs/Units/Source/SlyvStream.cpp",
    "../../Libs/Units/Source/SlyvString.cpp",
    "../../Libs/Units/Source/SlyvTime.cpp",
    "Syldeyn.cpp"
]

SyldeynIncludeDirs = [
    "../../../Libs/JCR6/Headers",
    "../../../Libs/Units/Headers"
]

SyldeynOutput = "SConsOut/%s/%%s"%platform.system()

SyldeynEnv = Environment(CPPPATH=SyldeynIncludeDirs)

SyldeynEnv.Program(SyldeynOutput%"syldeyn",SyldeynFiles)


