# WpdMtp
C#からMTPデバイスを操作するためのライブラリです。  
RICOH THETA Sなどのカメラを操作することができます。  

#### 使い方(概要)
1. MtpCommandのインスタンスを生成します。  
2. MtpCommandのGetDeviceIds()でPCに接続されているデバイスIDの一覧を取得します。  
3. MtpCommandのOpen()でデバイスに接続します。  
4. MtpCommandのExecute()でMTP通信を実行します。  
5. MtpCommandのClose()でデバイスを切断します。  

#### 注意点
1. いろんなところで予期せぬエラーが発生すると思います。ご使用は計画的かつ慎重に。  
2. OpenSessionとCloseSessionはWindowsが自動的に行っているようです(未確認)  
3. 64bitだとダメかもしれません(手元にないので不明。。)  

#### その他
実装には下記サイトをものすご～く参考にさせていただきました。  
https://blogs.msdn.microsoft.com/dimeby8/tag/wpd/
