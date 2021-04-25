namespace OpenTokFs.RequestDomain

type ProjectNameSetting = ProjectName of string | NoProjectName

type ProjectStatus = ActiveProjectStatus | SuspendedProjectStatus