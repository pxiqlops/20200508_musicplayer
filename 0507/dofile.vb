Imports System
Imports Shell32
Public Class dofile
    Dim _arlst As New List(Of String)  'リストの作成
    'フォルダーから音楽ファイルを取り出すと同時にファイルのタグ情報を抽出
    Public Sub search(pass As String)
        Dim ofolder As New IO.DirectoryInfo(pass)
        Dim ofile As IO.FileInfo

        Dim nme As String
        Dim Shell = New Shell()
        Dim f As Folder = Shell.NameSpace(pass)
        Dim item As FolderItem

        _arlst.Clear()

        For Each ofile In ofolder.GetFiles("*", IO.SearchOption.AllDirectories)
            item = f.ParseName(ofile.Name)

            'ファイル名から拡張子を削除する
            nme = IO.Path.GetFileNameWithoutExtension(ofile.FullName)

            'ダブルコーテーションカンマ区切りでarlstにAddする
            _arlst.Add("""" & ofile.FullName & """" & "," & """" & nme & """" & "," & """" & f.GetDetailsOf(item, 15) & """")
        Next
    End Sub

    'リストの読み込み
    Public ReadOnly Property arlst() As List(Of String)
        Get
            Return _arlst
        End Get
    End Property
End Class
