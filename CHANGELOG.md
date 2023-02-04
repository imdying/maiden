# Change Log
All notable changes to this project will be documented in this file.
</br>
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## [0.1.0] - 2023-02-04

 ### Added
  - Command `Rollback`.
  - Command `New` replaces `Restore` and `Init` commands.
  - Command `Build` should be able to execute externally. (`--help` for more info)

 ### Changed
  - Command `Init` has been reworked on. It should now just create configuration files.
  - Internal code has been rewrote.

 ### Removed
  - Command `Restore`.

## [0.0.0] - 2023-01-30

 ### Fixed
  - Outputs from shell invocations should display.