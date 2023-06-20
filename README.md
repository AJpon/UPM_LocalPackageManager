# UPM_LocalPackageManager
## 概要
Unity Package Manager(UPM)で、ローカルパッケージパスの記録方式を変更するパッケージです。

UPMでローカルパッケージを追加すると、パッケージのパスが`manifest.json`に記録されます。通常は絶対パスが記録されますが、`manifest.json`を直接変更することで相対パスを使用することもできます。  
しかし、パスの記録方式の変更や確認は手作業で行う必要がありました。

本パッケージはUPMでローカルパッケージを追加するときに、パスの記録方式を選択できるようにします。  
また、UPMで選択した追加済みローカルパッケージの記録パスをGUIで確認、変更できるようにします。

## Install
Install via UPM  
```
https://github.com/AJpon/UPM_LocalPackageManager.git
```
