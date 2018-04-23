Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.ConditionalAppearance
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace WinSolution.Module

    <NavigationItem, Appearance("Can Edit Items", TargetItems := "Items", Criteria := "!CanEdit", Enabled := False)> _
    Public Class OwnerClass
        Inherits BaseObject

        Public Sub New(ByVal s As Session)
            MyBase.New(s)
        End Sub
        Private _CanEdit As Boolean
        <ImmediatePostData> _
        Public Property CanEdit() As Boolean
            Get
                Return _CanEdit
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue(Of Boolean)("CanEdit", _CanEdit, value)
            End Set
        End Property
        <Aggregated, Association> _
        Public ReadOnly Property Items() As XPCollection(Of ItemCLass)
            Get
                Return GetCollection(Of ItemCLass)("Items")
            End Get
        End Property
    End Class

    <DefaultListViewOptions(True, DevExpress.ExpressApp.NewItemRowPosition.None), Appearance("Can Edit Value", TargetItems := "Value", Criteria := "not (Owner.CanEdit and CanEdit)", Enabled := False)> _
    Public Class ItemCLass
        Inherits HCategory

        Public Sub New(ByVal s As Session)
            MyBase.New(s)
        End Sub
        Private _Owner As OwnerClass
        <Association> _
        Public Property Owner() As OwnerClass
            Get
                Return _Owner
            End Get
            Set(ByVal value As OwnerClass)
                SetPropertyValue(Of OwnerClass)("Owner", _Owner, value)
            End Set
        End Property
        Private _CanEdit As Boolean
        <ImmediatePostData> _
        Public Property CanEdit() As Boolean
            Get
                Return _CanEdit
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue(Of Boolean)("CanEdit", _CanEdit, value)
            End Set
        End Property
        Private _Value As Integer
        Public Property Value() As Integer
            Get
                Return _Value
            End Get
            Set(ByVal value As Integer)
                SetPropertyValue(Of Integer)("Value", _Value, value)
            End Set
        End Property
    End Class

End Namespace
