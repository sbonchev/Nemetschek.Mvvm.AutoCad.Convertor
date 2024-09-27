# Nemetschek.Mvvm.AutoCad.LayersConvertor

## 1. Draw AutoCad Primitive Drawing Functionalities
*  Lines
*  Circles
*  Select Entity
- ...
## 2. AutoCad Entity Layers Convertor
*  Load selected drawing (.dwg) chart-files
*  Load all layers to selected chart-file
*  Conver one drawing layer to theother one:
  - Select a layer (from destination) and another one from the same schema (end destination),
    with imlementation logic(very similar to previous one, see - EDD AutoCad.LayersConvertor proj.)
## 3. WPF MVVM Pattern GUI Implemetation
*  Models
*  ModelView
*  Services (BL seperation)
*  Views
- MVVM pattern implementation
- Execute Comand pattern implementation
## 4. Create AutoCad PlugIns IExtensionApplication Implementation
## 5. IoC Services implementation - TODO
## LayersConvertor Instalation For Testing
*  Must have AutoCad installed.
*  Add AutoCad dll project reference to - accore.dll, acdbmgd.dll, acmgd.dll
*  For debug -  Add process startup reference to acad.exe (AutoCad executable)
