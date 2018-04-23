Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.Xpo

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports System.ComponentModel
Imports DevExpress.Persistent.Base.General
Imports DevExpress.Data.Filtering

Namespace WinSolution.Module
	<NavigationItem> _
	Public MustInherit Class Category
		Inherits BaseObject
		Implements ITreeNode
		Private name_Renamed As String
		Protected MustOverride ReadOnly Property Parent() As ITreeNode
		Protected MustOverride ReadOnly Property Children() As IBindingList
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Property Name() As String
			Get
				Return name_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", name_Renamed, value)
			End Set
		End Property
		Private _person As Person
		Public Property Person() As Person
			Get
				Return _person
			End Get
			Set(ByVal value As Person)
				SetPropertyValue("Person", _person, value)
			End Set
		End Property
		#Region "ITreeNode"
		Private ReadOnly Property ITreeNode_Children() As IBindingList Implements ITreeNode.Children
			Get
				Return Children
			End Get
		End Property
		Private ReadOnly Property ITreeNode_Name() As String Implements ITreeNode.Name
			Get
				Return Name
			End Get
		End Property
		Private ReadOnly Property ITreeNode_Parent() As ITreeNode Implements ITreeNode.Parent
			Get
				Return Parent
			End Get
		End Property
		#End Region
	End Class

	Public Class ProjectGroup
		Inherits Category
		Protected Overrides ReadOnly Property Parent() As ITreeNode
			Get
				Return Nothing
			End Get
		End Property
		Protected Overrides ReadOnly Property Children() As IBindingList
			Get
				Return Projects
			End Get
		End Property
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Sub New(ByVal session As Session, ByVal name As String)
			MyBase.New(session)
			Me.Name = name
		End Sub
		<Association("ProjectGroup-Projects"), Aggregated> _
		Public ReadOnly Property Projects() As XPCollection(Of Project)
			Get
				Return GetCollection(Of Project)("Projects")
			End Get
		End Property
	End Class

	Public Class Project
		Inherits Category
		Private projectGroup_Renamed As ProjectGroup
		Protected Overrides ReadOnly Property Parent() As ITreeNode
			Get
				Return projectGroup_Renamed
			End Get
		End Property
		Protected Overrides ReadOnly Property Children() As IBindingList
			Get
				Return ProjectAreas
			End Get
		End Property
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Sub New(ByVal session As Session, ByVal name As String)
			MyBase.New(session)
			Me.Name = name
		End Sub
		<Association("ProjectGroup-Projects")> _
		Public Property ProjectGroup() As ProjectGroup
			Get
				Return projectGroup_Renamed
			End Get
			Set(ByVal value As ProjectGroup)
				projectGroup_Renamed = value
				SetPropertyValue("ProjectGroup", projectGroup_Renamed, value)
			End Set
		End Property
		<Association("Project-ProjectAreas"), Aggregated> _
		Public ReadOnly Property ProjectAreas() As XPCollection(Of ProjectArea)
			Get
				Return GetCollection(Of ProjectArea)("ProjectAreas")
			End Get
		End Property
	End Class

	Public Class ProjectArea
		Inherits Category
		Private project_Renamed As Project
		Protected Overrides ReadOnly Property Parent() As ITreeNode
			Get
				Return project_Renamed
			End Get
		End Property
		Protected Overrides ReadOnly Property Children() As IBindingList
			Get
				Return New BindingList(Of Object)()
			End Get
		End Property
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Sub New(ByVal session As Session, ByVal name As String)
			MyBase.New(session)
			Me.Name = name
		End Sub
		<Association("Project-ProjectAreas")> _
		Public Property Project() As Project
			Get
				Return project_Renamed
			End Get
			Set(ByVal value As Project)
				project_Renamed = value
				SetPropertyValue("Project", project_Renamed, value)
			End Set
		End Property
	End Class
End Namespace
