   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:QuickChargeDisplay.ControlQuickChargeDisplay.Control  7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  &     QuickChargeDisplay.IsolationPage   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    :control(clr-namespace:QuickChargeDisplay.Control    -my clr-namespace:QuickChargeDisplay    -     N� Title$  IsolationCheck��$	��800��$	��480�� N� 
Background ��  �� ImageSource$: 4image\04_충전중화면\04_1_절연저항측정.gif��.��� .+�.   (QuickChargeDisplay.Control.AnimatedImage  -     Name$ 	BackImage��$��3��$��2���� -   $��BmsInfo��$��15(�
�������$��BMS Info��$	��155��$��356,577,216,36q�$	��451��y���  �� Path$ Visible�   QuickChargeDisplay.QCDV   BmsInfo �� Source# Z  �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.���� $	��109��	 �� Orientation$	 Vertical=�$	��408��.+��� $	 
Horizontal=�.+��� $��13(�
 �� TextAlignment$
 Right=�$	��120��
전압 :�� $��13(�$��80������ $ Data1�# Z $ TwoWay=�$ PropertyChanged=��� $��13(�$
 Right=�$	��120��
전류 :�� $��13(�$��80������ $ Data21�# Z $ TwoWay=�$ PropertyChanged=��� 
�������$��2��$	 
Horizontal=��� $	 
Horizontal=�.+��� $��12(�$
 Right=�$	��120��ChargingEnabled :�� $��13(�$��80������ $ Data2�# Z $ TwoWay=�$ PropertyChanged=��� $��12(�$
 Right=�$	��120��MaxBatteryVoltage :�� $��13(�$��80������ $ Data22�# Z $ TwoWay=�$ PropertyChanged=��� $	 
Horizontal=�.+��� $��12(�$
 Right=�$	��120��ShiftPosition :�� $��13(�$��80������ $ Data3�# Z $ TwoWay=�$ PropertyChanged=��� $��12(�$
 Right=�$	��120��Target Volt[V] :�� $��13(�$��80������ $ Data23�# Z $ TwoWay=�$ PropertyChanged=��� $	 
Horizontal=�.+��� $��12(�$
 Right=�$	��120��SystemFault :�� $��13(�$��80������ $ Data4�# Z $ TwoWay=�$ PropertyChanged=��� $��12(�$
 Right=�$	��120��Target Current[A] :�� $��13(�$��80������ $ Data24�# Z $ TwoWay=�$ PropertyChanged=��� $	 
Horizontal=�.+��� $��12(�$
 Right=�$	��120��BmsVehicle :�� $��13(�$��80������ $ Data5�# Z $ TwoWay=�$ PropertyChanged=��� $��12(�$
 Right=�$	��120���� $��13(�$��80������ $ Data25�# Z $ TwoWay=�$ PropertyChanged=��� $	 
Horizontal=�.+��� $��12(�$
 Right=�$	��120��NormalStopRequest :�� $��13(�$��80������ $ Data6�# Z $ TwoWay=�$ PropertyChanged=��� $��12(�$
 Right=�$	��120��TotalBatteryCapacity :�� $��13(�$��80������ $ Data26�# Z $ TwoWay=�$ PropertyChanged=��� $	 
Horizontal=�.+��� $��12(�$
 Right=�$	��120���� $��13(�$��80������ $ Data7�# Z $ TwoWay=�$ PropertyChanged=��� $��12(�$
 Right=�$	��120��RemainingBatteryEnergy :�� $��13(�$��80������ $ Data27�# Z $ TwoWay=�$ PropertyChanged=��� -   $��IsolationCancel��$
��Left=�$��356,414,0,23q�$.��(image\02_커넥터연결취소버튼.png��$	��139��