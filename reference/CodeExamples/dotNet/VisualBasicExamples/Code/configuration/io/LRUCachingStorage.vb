Imports Db4objects.Db4o.Internal.Caching
Imports Db4objects.Db4o.IO

Namespace Db4oDoc.Code.Configuration.IO
    ' #example: Exchange the cache-implementation
    Public Class LRUCachingStorage
        Inherits CachingStorage
        Private ReadOnly pageCount As Integer

        Public Sub New(ByVal storage As IStorage)
            MyBase.New(storage)
            Me.pageCount = 128
        End Sub

        Public Sub New(ByVal storage As IStorage, ByVal pageCount As Integer, ByVal pageSize As Integer)
            MyBase.New(storage, pageCount, pageSize)
            Me.pageCount = pageCount
        End Sub

        Protected Overrides Function NewCache() As ICache4
            Return CacheFactory.NewLRUCache(pageCount)
        End Function
    End Class
    ' #end example
End Namespace