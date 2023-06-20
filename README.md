# UPM_LocalPackageManager
## 概要
Unity Package Manager(UPM)で、ローカルパッケージパスの記録方式を変更するパッケージです。

UPMでローカルパッケージを追加すると、パッケージのパスが`manifest.json`に記録されます。通常は絶対パスが記録されますが、`manifest.json`を直接変更することで相対パスを使用することもできます。  
しかし、パスの記録方式の変更や確認は手作業で行う必要がありました。

本パッケージはUPMでローカルパッケージを追加するときに、パスの記録方式を選択できるようにします。  
また、UPMで選択した追加済みローカルパッケージの記録パスをGUIで確認、変更できるようにします。

## インストール
UPMを使用してインストールします。
```
https://github.com/AJpon/UPM_LocalPackageManager.git
```

1. `Window` > `Package Manager` を開きます。
2. `+` > `Add package from git URL...` を選択します。
3. ```https://github.com/AJpon/UPM_LocalPackageManager.git``` を入力し、`Add` を押すことで最新版がインストールされます。

## 使い方
UPM上から専用のウィンドウを開くことができます。また、このウィンドウはローカルパッケージの追加時に自動で開きます。  
ここでは`manifest.json`の`dependencies`に記録されたパスをGUIで確認、変更できます。


<!-- 
・UPMでRemoveボタンの横のEditを押すとインスペクタが出る
・インスペクタから情報の確認、manifest.json の修正, Openができる
UPM周辺で完結させたいのでこういう感じになってる 
-->

