#!/usr/bin/env janet

(use cbt)

(build-metadata
  :qud-dlls "/home/petrak/.local/share/Steam/steamapps/common/Caves of Qud/CoQ_Data/Managed/"
  :qud-mods-folder "/home/petrak/.config/unity3d/Freehold Games/CavesOfQud/Mods/")

(declare-mod
  "its-so-sad-resheph-died-of-ligma"
  "It's So Sad Resheph Died of Ligma"
  "petrak@"
  "0.1.0"
  :description "Who's Resheph?")

# (set-debug-output true)

