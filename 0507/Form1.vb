Public Class Form1
    Dim flework As New dofile               'Class dofileのインスタンス
    Dim checindex As Integer                'チェックしたインデックス
    Dim indexcount As Integer               'チェックしたインデックスの値の取得
    Dim arnme As New List(Of String)        'ファイルのリスト
    Dim selectindx As New List(Of Integer)  'チェックしたインデックスのリスト
    Dim pass As String  '任意フォルダーパス
    Dim item() As String   '各項目の配列
    Private Sub f() Handles MyBase.Shown

        '各ボタンの状態の変更
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        Button5.Enabled = False
        Button7.Enabled = False

        'MediaPlayerを非表示にする（デザイナー画面でも変更できます）
        AxWindowsMediaPlayer1.Visible = False

        'pass = "C:\music"
        ''音楽ファイルをCheckedListBoxに表示
        'flework.search(pass)       'ファイルの読み込み

        ''flework.arlstはClass dofileで作成したコレクションです。
        'For Each str As String In flework.arlst
        '    str = str.Replace("""", "")          '""を取る
        '    item = str.Split(",")                'strをカンマで区切る
        '    CheckedListBox1.Items.Add(item(1))   'CheckedListBoxにitem(1)を表示
        '    arnme.Add(item(0))                   'arnmeにitem(0)をAddする
        'Next

    End Sub
    '再生ボタン
    Private Sub Button1_Click() Handles Button1.Click
        If selectindx.Count = 0 Then
            MsgBox("ファイルが選択されていません。")
            Return
        End If

        '再生のメソッド
        doplay()
    End Sub

    'Timer1_Tickイベント
    Private Sub Timer1_Tick() Handles Timer1.Tick
        '再生位置をスクロールバーのValueへ入力します。
        HScrollBar1.Value = AxWindowsMediaPlayer1.Ctlcontrols.currentPosition
    End Sub
    'HScrollBar1_Scrollイベント
    Private Sub HScrollBar1_Scroll(sender As System.Object, e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar1.Scroll
        '再生位置にスクロールを移動した位置を入力
        AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = CDbl(HScrollBar1.Value)
    End Sub

    '停止ボタン
    Private Sub Button3_Click() Handles Button3.Click
        AxWindowsMediaPlayer1.Ctlcontrols.stop()

        '各ボタンの状態の変更
        Button1.Enabled = True
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        Button7.Enabled = False
    End Sub

    '一時停止ボタン
    Private Sub Button4_Click() Handles Button4.Click
        '一時停止した位置をonestopに代入(HScrollBar1_Scrollが無いときは必要)
        AxWindowsMediaPlayer1.Ctlcontrols.pause()

        '各ボタンの状態の変更
        Button1.Enabled = True
        Button2.Enabled = False
        Button3.Enabled = False
        Button7.Enabled = False
    End Sub

    '次の曲へボタン
    Private Sub Button2_Click() Handles Button2.Click
        If indexcount = 1 Then
            MsgBox("他のファイルは選択されていません。")
            Return
        End If

        '次の曲
        AxWindowsMediaPlayer1.Ctlcontrols.next()
    End Sub

    '前の曲へボタン
    Private Sub Button7_Click() Handles Button7.Click
        If indexcount = 1 Then
            MsgBox("他のファイルは選択されていません。")
            Return
        End If

        '前の曲
        AxWindowsMediaPlayer1.Ctlcontrols.previous()
    End Sub

    '楽曲情報表示処理
    Private Sub AxWindowsMediaPlayer1_PlayStateChange(sender As System.Object,
            e As AxWMPLib._WMPOCXEvents_PlayStateChangeEvent) Handles AxWindowsMediaPlayer1.PlayStateChange
        '演奏中
        If e.newState = 3 Then
            Timer1.Enabled = True 'Timer1のスタート
            Timer1.Interval = 1   'Timer1のインターバル

            'HScrollBar1の上限最大値を入力する
            HScrollBar1.Maximum = CInt(AxWindowsMediaPlayer1.currentMedia.duration)

            Dim artnme As String  'アーティスト名
            Dim title As String   'タイトル
            Dim madetime As String = ""  '年月日
            Dim tim As Double     '演奏時間
            Dim pas As String     'ファイルのフルパス
            Dim item() As String   '各項目の配列

            Dim min As Double 　'再生時間（分）
            Dim sec As Double 　'再生時間（秒）

            '各値を取得
            artnme = AxWindowsMediaPlayer1.currentMedia.getItemInfo("Author")
            title = AxWindowsMediaPlayer1.currentMedia.name
            tim = AxWindowsMediaPlayer1.currentMedia.duration
            pas = AxWindowsMediaPlayer1.currentMedia.sourceURL.ToString
            For Each str As String In flework.arlst
                str = str.Replace("""", "")
                item = str.Split(",")
                If item(0) = pas Then
                    madetime = item(2)
                End If
            Next

            min = Math.Round(tim, 0) \ 60
            sec = Math.Round(tim, 0) Mod 60

            With ListBox1.Items
                .Clear()        'ListBox1のクリア
                .Add("アーティスト" & "   " & artnme)
                .Add("タイトル    " & "   " & title)
                .Add("年          " & "   " & madetime)
                '.Add("演奏時間 " & "   " & Math.Round(tim, 0) & "秒")  '少数点以下は五捨六入
                .Add("演奏時間 " & "   " & min & "分" & sec & "秒")  '少数点以下は五捨六入
            End With
        End If

        '演奏終了
        If e.newState = 8 Then '登録ファイルが1個の場合e.newStateの戻り値が8の時の処理
            Timer1.Enabled = False 'Timer1の監視を停止
            If CheckBox1.Checked = True Then
                '再生のメソッド
                doplay()
            ElseIf CheckBox1.Checked = False Then
                '各ボタンの状態の変更
                Button1.Enabled = True
                Button2.Enabled = True
                Button3.Enabled = True
                Button4.Enabled = True
                Button7.Enabled = True
            End If

        ElseIf e.newState = 1 Then  '登録ファイルが1個の場合e.newStateの戻り値が1の時の処理
            Timer1.Enabled = False 'Timer1の監視を停止
            If CheckBox1.Checked = True Then
                If indexcount = 1 Then '登録ファイルが1個の場合の処理
                    '再生のメソッド
                    doplay()
                ElseIf CheckBox1.Checked = False Then
                    '各ボタンの状態の変更
                    Button1.Enabled = True
                    Button2.Enabled = False
                    Button3.Enabled = False
                    Button4.Enabled = False
                    Button7.Enabled = False
                End If
            End If
        End If
    End Sub
    'CheckedListBox1_ItemCheckイベント
    Private Sub CheckedListBox1_ItemCheck(sender As System.Object,
         e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck

        'CheckedListBox1で選んだファイルのインデックス
        checindex = CheckedListBox1.SelectedIndex
        'チェックされた場合リストに追加(Add)
        If e.CurrentValue = 0 Then
            selectindx.Add(checindex)
        End If
        'チェックが外された場合リストから削除(Remove)
        If e.CurrentValue = 1 Then
            selectindx.Remove(checindex)
        End If

        selectindx.Sort()   'selectindxを数字の小さい順にソートする（クイックソート）

        'currentPlaylistのクリア
        AxWindowsMediaPlayer1.currentPlaylist.clear()

        Dim indexnum As Integer
        'チェックしたインデックスのリストの数をカウントする 
        indexcount = selectindx.Count
        For num As Integer = 0 To indexcount - 1
            indexnum = selectindx(num)   'チェックしたインデックスの値の取得

            'AxWindowsMediaPlayer1に再生するファイルの登録
            AxWindowsMediaPlayer1.currentPlaylist.appendItem _
                (AxWindowsMediaPlayer1.newMedia(arnme(indexnum)))
        Next

        '停止のメソッド
        dostop()
    End Sub

    '再生のメソッド
    Private Sub doplay()
        AxWindowsMediaPlayer1.Ctlcontrols.play()

        '各ボタンの状態の変更
        Button1.Enabled = False
        Button2.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
        Button7.Enabled = True
    End Sub

    '停止のメソッド
    Private Sub dostop()
        'このイベントが発生した場合演奏をストップする
        AxWindowsMediaPlayer1.Ctlcontrols.stop()

        '各ボタンの状態の変更
        Button1.Enabled = True
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        Button7.Enabled = False
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        For index = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(index, False)
        Next
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged
        Button5.Enabled = True
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        'pass = "C:\music2"
        CheckedListBox1.Items.Clear()
        arnme.Clear()

        Dim fbd As New FolderBrowserDialog

        '上部に表示する説明テキスト
        fbd.Description = "フォルダを指定してください。"
        'ルートフォルダを指定する,デフォルトでDesktop
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        '最初に選択するフォルダを指定する
        'RootFolder以下にあるフォルダである必要がある
        fbd.SelectedPath = "C:\Windows"
        'ユーザーが新しいフォルダを作成できるようにするためTrue
        fbd.ShowNewFolderButton = True

        'ダイアログを表示する
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            '選択されたフォルダを表示する
            pass = fbd.SelectedPath
        End If

        flework.search(pass)
        'flework.arlstはClass dofileで作成したコレクションです。
        For Each str As String In flework.arlst
            str = str.Replace("""", "")          '""を取る
            item = str.Split(",")                'strをカンマで区切る
            CheckedListBox1.Items.Add(item(1))   'CheckedListBoxにitem(1)を表示
            arnme.Add(item(0))                   'arnmeにitem(0)をAddする
        Next

    End Sub

End Class

