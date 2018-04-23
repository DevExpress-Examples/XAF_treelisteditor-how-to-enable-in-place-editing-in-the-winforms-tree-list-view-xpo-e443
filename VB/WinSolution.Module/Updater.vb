Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl

Namespace WinSolution.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal session As Session, ByVal currentDBVersion As Version)
			MyBase.New(session, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Dim p As New Person(Session)
			p.FirstName="Person1"
			Dim pg As New ProjectGroup(Session, "ProjectGroup1")
			pg.Person = p
			Dim pr As New Project(Session, "Project1")
			pr.Person = p
			pg.Projects.Add(pr)
			Dim pa As New ProjectArea(Session, "ProjectArea1")
			pa.Person = p
			pr.ProjectAreas.Add(pa)
			p.Save()
			pa.Save()
			pr.Save()
			pg.Save()
		End Sub
	End Class
End Namespace
