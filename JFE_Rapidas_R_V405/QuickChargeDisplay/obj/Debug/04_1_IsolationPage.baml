   M S B A M L   `   `   `  ����M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:QuickChargeDisplay.ControlQuickChargeDisplay.Control  5      7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  5   5   &     QuickChargeDisplay.IsolationPage   5      R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      :control(clr-namespace:QuickChargeDisplay.Control    5      -my clr-namespace:QuickChargeDisplay    5   5   -   5          N� Title$  IsolationCheck��5      $	��800��6   $	��480��6(    N� 
Background 5      �� 5	   
    �� ImageSource$: 4image\04_충전중화면\04_1_절연저항측정.gif��6   6D   .��5      � .+�5   
   .   (QuickChargeDisplay.Control.AnimatedImage  -   5          Name$ 	BackImage��5       $��3��63   $��2��6G   6Z   �� 5   
   -   5        $��BmsInfo��5   k   $��15(�6   
�������$��BMS Info��64   $	��155��6F   $��356,577,216,36q�6S   $	��451��6�   y�6z   ��  �� Path$ Visible�   QuickChargeDisplay.QCDV   BmsInfo �� Source# Z  �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.��5      �� $	��109��6   	 �� Orientation$	 Vertical=�6&   $	��408��6=   .+�5      �� $	 
Horizontal=�6   .+�5      �� $��13(�6    
 �� TextAlignment$
 Right=�6.   $	��120��6D   
전압 :6V   �� 5      $��13(�6    $��80��6�   ��6.   �� $ Data1�# Z $ TwoWay=�$ PropertyChanged=�6�   �� 5      $��13(�6    $
 Right=�6.   $	��120��6D   
전류 :6V   �� 5      $��13(�6    $��80��6�   ��6.   �� $ Data21�# Z $ TwoWay=�$ PropertyChanged=�6�   5      �� 5      
�������$��2��6/   $	 
Horizontal=�6:   �� 5      $	 
Horizontal=�6   .+�5      �� $��12(�6    $
 Right=�6.   $	��120��6D   ChargingEnabled :6c   �� 5      $��13(�6    $��80��6�   ��6.   �� $ Data2�# Z $ TwoWay=�$ PropertyChanged=�6�   �� 5      $��12(�6    $
 Right=�6.   $	��120��6D   MaxBatteryVoltage :6e   �� 5      $��13(�6    $��80��6�   ��6.   �� $ Data22�# Z $ TwoWay=�$ PropertyChanged=�6�   5      �� 5      $	 
Horizontal=�6   .+�5      �� $��12(�6    $
 Right=�6.   $	��120��6D   ShiftPosition :6a   �� 5      $��13(�6    $��80��6�   ��6.   �� $ Data3�# Z $ TwoWay=�$ PropertyChanged=�6�   �� 5       $��12(�6    $
 Right=�6.   $	��120��6D   Target Volt[V] :6b   �� 5!      $��13(�6    $��80��6�   ��6.   �� $ Data23�# Z $ TwoWay=�$ PropertyChanged=�6�   5"      �� 5#      $	 
Horizontal=�6   .+�5$      �� $��12(�6    $
 Right=�6.   $	��120��6D   SystemFault :6_   �� 5%      $��13(�6    $��80��6�   ��6.   �� $ Data4�# Z $ TwoWay=�$ PropertyChanged=�6�   �� 5&      $��12(�6    $
 Right=�6.   $	��120��6D   Target Current[A] :6e   �� 5'      $��13(�6    $��80��6�   ��6.   �� $ Data24�# Z $ TwoWay=�$ PropertyChanged=�6�   5(      �� 5)      $	 
Horizontal=�6   .+�5*      �� $��12(�6    $
 Right=�6.   $	��120��6D   BmsVehicle :6^   �� 5+      $��13(�6    $��80��6�   ��6.   �� $ Data5�# Z $ TwoWay=�$ PropertyChanged=�6�   �� 5,      $��12(�6    $
 Right=�6.   $	��120��6D   �� 5-      $��13(�6    $��80��6�   ��6.   �� $ Data25�# Z $ TwoWay=�$ PropertyChanged=�6�   5.      �� 5/      $	 
Horizontal=�6   .+�50      �� $��12(�6    $
 Right=�6.   $	��120��6D   NormalStopRequest :6e   �� 51      $��13(�6    $��80��6�   ��6.   �� $ Data6�# Z $ TwoWay=�$ PropertyChanged=�6�   �� 52      $��12(�6    $
 Right=�6.   $	��120��6D   TotalBatteryCapacity :6h   �� 53      $��13(�6    $��80��6�   ��6.   �� $ Data26�# Z $ TwoWay=�$ PropertyChanged=�6�   54      �� 55      $	 
Horizontal=�6   .+�56      �� $��12(�6    $
 Right=�6.   $	��120��6D   �� 57      $��13(�6    $��80��6�   ��6.   �� $ Data7�# Z $ TwoWay=�$ PropertyChanged=�6�   �� 58      $��12(�6    $
 Right=�6.   $	��120��6D   RemainingBatteryEnergy :6j   �� 59      $��13(�6    $��80��6�   ��6.   �� $ Data27�# Z $ TwoWay=�$ PropertyChanged=�6�   5:      5;      5<      �� 5=   
   -   5        $��IsolationCancel��5=   m   $
��Left=�6   $��356,414,0,23q�6+   $.��(image\02_커넥터연결취소버튼.png��6A   $	��139��6a   6�   5>      5?      