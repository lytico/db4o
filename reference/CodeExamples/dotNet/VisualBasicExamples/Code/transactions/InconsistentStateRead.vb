Imports System.IO
Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Transactions
    Public Class InconsistentStateRead
        Private Const InitialMoneyOnOneAccount As Integer = 1000
        Private Const DatabaseFile As String = "database.db4o"
        Private ReadOnly rootContainer As IObjectContainer

        Public Shared Sub Main(args As String())
            File.Delete(DatabaseFile)
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim startInstance = New InconsistentStateRead(container)
                startInstance.Main()
            End Using
        End Sub

        Public Sub New(rootContainer As IObjectContainer)
            Me.rootContainer = rootContainer
        End Sub

        Private Sub Main()
            StoreInitialBankAccounts()

            ShowIsolationIssue()
        End Sub

        Private Sub ShowIsolationIssue()
            InTransaction(Function(container)
                              ' #example: We list the bank accounts and sum up the money
                              Dim moneyInOurAccounts As Long = 0
                              Dim bankAccounts = From b As BankAccount In container Select b
                              For Each account As BankAccount In bankAccounts
                                  Console.WriteLine("This account has " & account.Money)
                                  moneyInOurAccounts += account.Money
                                  MoveMoneyTransactionFinishes()
                              Next
                              ' We get the wrong answer here
                              ' #end example
                              Console.WriteLine("The money total is {0}. Expected is {1}", moneyInOurAccounts, InitialMoneyOnOneAccount * bankAccounts.Count())

                          End Function)
        End Sub

        Private Sub MoveMoneyTransactionFinishes()
            InTransaction(Function(container)
                              ' #example: Meanwhile we transfer money.
                              Dim bankAccounts = From b As BankAccount In container Select b
                              Dim debitAccount = bankAccounts.ElementAt(0)
                              Dim creditAccount = bankAccounts.ElementAt(1)

                              Dim moneyToTransfer As Integer = 200
                              creditAccount.WithDraw(moneyToTransfer)
                              debitAccount.Deposit(moneyToTransfer)

                              container.Store(debitAccount)
                              container.Store(creditAccount)
                              ' #end example
                              container.Commit()

                          End Function)
        End Sub

        Private Sub StoreInitialBankAccounts()
            InTransaction(Function(container)
                              container.Store(New BankAccount("Account A", InitialMoneyOnOneAccount))
                              container.Store(New BankAccount("Account B", InitialMoneyOnOneAccount))

                          End Function)
        End Sub

        Private Sub InTransaction(operation As Action(Of IObjectContainer))
            Using container As IObjectContainer = Me.rootContainer.Ext().OpenSession()
                operation(container)
            End Using
        End Sub
    End Class

    Friend Class BankAccount
        Private m_accountName As String
        Private moneyOnAccountInCents As Long

        Public Sub New(accountName As String, moneyOnAccountInCents As Long)
            Me.m_accountName = accountName
            Me.moneyOnAccountInCents = moneyOnAccountInCents
        End Sub

        Public Sub Deposit(amount As Long)
            moneyOnAccountInCents += amount
        End Sub

        Public Sub WithDraw(amount As Long)
            moneyOnAccountInCents -= amount
        End Sub

        Public ReadOnly Property AccountName() As String
            Get
                Return m_accountName
            End Get
        End Property


        Public ReadOnly Property Money() As Long
            Get
                Return moneyOnAccountInCents
            End Get
        End Property
    End Class
End Namespace
