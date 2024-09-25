# Nemetschek.Mvvm.AutoCad.LayersConvertor

## 1. Drawing AutoCad Primitive Drawing Functionalities
* - Lines
* - Circles
* - Select Entity
- ...
## 2. AutoCad Entity Layers Convertor
* - Load selected drawing (.dwg) chart-files
* - Load all layers to selected chart-file
* - Select layer (from destination) and converted layer(end destination).
    Imlementation logic(very similar to previous one, see - AutoCad.LayersConvertor proj.)
## 3. WPF MVVM Pattern GUI Implemetation
* - Models
* - ModelView
* - Services (BL seperation)
* - Views
* - Comand pattern implementation
## 4. Create AutoCad PlugIns IExtensionApplication Implementation
## 5. IoC Services implementation - TODO
## LayersConvertor Instalation For Testing
* - Must have AutoCad installed.
* - Add AutoCad dll project reference to - accore.dll, acdbmgd.dll, acmgd.dll
* - For debug -  Add process startup reference to acad.exe (AutoCad executable)
