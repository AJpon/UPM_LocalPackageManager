# UPM_LocalPackageManager
## 概要
Unity Package Manager(UPM)で、ローカルパッケージパスの記録方式を変更するパッケージです。

UPMでローカルパッケージを追加すると、パッケージのパスが`manifest.json`に記録されます。通常は絶対パスが記録されますが、`manifest.json`を直接変更することで相対パスを使用することもできます。  
しかし、パスの記録方式の変更や確認は手作業で行う必要がありました。

本パッケージはUPMでローカルパッケージを追加するときに、パスの記録方式を選択できるようにします。  
また、UPMで選択した追加済みローカルパッケージの記録パスをGUIで確認、変更できるようにします。

## Install
UPMを使用してインストールします。

1. `Window` > `Package Manager` を開きます。
2. `+` > `Add package from git URL...` を選択します。
3. ```https://github.com/AJpon/UPM_LocalPackageManager.git``` を入力し、`Add` を押します。

```
https://github.com/AJpon/UPM_LocalPackageManager.git
```
