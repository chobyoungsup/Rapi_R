   M S B A M L   `   `   `  ����M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  5      7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  5      %    QuickChargeDisplay.ChargingPage   5      R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      <convert*clr-namespace:QuickChargeDisplay.Converter    5      0local clr-namespace:QuickChargeDisplay    5      -   5          N� Title$  ChargingPage��5      $	��800��6   $	��480��6&    � 	Resources 5
      6   0QuickChargeDisplay.Converter.SocPercentConverter   SocPercentConverter;   5QuickChargeDisplay.Converter.DoublePrecisionConverter  doublePrecisionConverter �� 
TargetType �� Property �� Value��%  &	        &	      '�� &       5   
   6&     5   
   6+   �� 5   
   " ��6   .�5      ��  � ��$ font\DroidSans-Bold.ttf)�6+   �� 5       � ��$
 40pt(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��
 �����6+   �� 5       � ��
 ���� 6+   �� 5       � ��
 ���� 6,   �� 5       � ��$ Bottom=�69   �� 5       � ��$ Right=�6;   �� 5       �@��TextAlignment$ Right=�6.   �� 5       � |� .  6*   5      5       N� 
Background 5      �� 5   
    �� ImageSource$/ )image\04_충전중화면\04_충전중.jpg��6   6?   .��5      � .+�5    
   (   "QuickChargeDisplay.ExtendedBinding     Source 6    ��  �� Path$ ChargingSoc�   QuickChargeDisplay.QCDV	   Charging
 �� Source#
 Z	    Target 5!       �� $ 
SocPercent� �� RelativeSource �� *FindAncestor+ �� AncestorType" N��� 5#   
   .+�5%      �� $E��?image\04_충전중화면\배터리화면\자동차아이콘.png��6   $
��Fill=�6=   $	��120��6L   $	��121��6X    �� Left$	 166��6e   
 �� Top$	 201��6w    K� ZIndex$ 3��6�   �� 5'      -   5         ��Name$ remainChargeMinute��5'       �� TextAlignment$ Right=�5(   �   $	��136��6�   $	 544��6�   $	 172��6�   $��66.25��6�   $��40(�6�   ��6   �� $ 
RemainTime�#
 Z	  �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5)      -   5        $ chargingMoney��5)      $ Right=�5*   �   $	��136��6�   $ 24��6�   $	 291��6�   $��60��6�   $��40(�6�   $y�Hidden=�6�   ��6   �� $ ChargingMoney�#
 Z	 $ TwoWay=�$ PropertyChanged=�6�   �� 5+      -   5        $ chargingCurrent��5+      $ Right=�5,   �   $	��136��6�   $	 544��6�   $	 243��6�   $��66.25��6�   $��40(�6�   ��6   �� $ ChargingCurrent�#
 Z	 $ TwoWay=�$ PropertyChanged=�6�   �� 5-      -   5        $ chargingKwh��5-      $ Right=�50   ;   $	��136��6Q   $	 544��6]   $	 312��6o   $��66.25��6�   $��40(�6�   ��5.      �� $ ChargingWatt�#
 Z	 $ TwoWay=�$ PropertyChanged=� �� 	Converter# [  �� ConverterParameter$ {0:F1}��50   �   �� 51      -   5        $ chargingSoc��51      $ Right=�54      $	��136��6,   $	 544��68   $	 384��6J   $��66.25��6[   $��40(�6j   ��51   +   �� $ ChargingSoc�#
 Z	 $ TwoWay=�$ PropertyChanged=�# [ $ {0:F1}��54   j   �� 57      -   5         ��Name$ abortCharge��57      $5��/image\04_충전중화면\충전중지버튼.png��6K   $	��265��6n   $��96��6z   $	 100��6�   $	 354��6�   6�   �� 5<      -   5        $��BmsInfo��5<      $��Charger Info��6&   
�������$��15(�5=   *   $ 24��68   $	 155��6I   $��52��6Z   $	��429��6f   y�5<   <   �� $ Visible�   BmsInfo#
 Z $ TwoWay=�$ PropertyChanged=�.��5>      ��  �� Orientation$ Vertical=�6   $	��109��64   $	��408��6A   .+�5C      �� $ 
Horizontal=�6!   .+�5D      �� $��13(�6$   $	��120��62    �� TextAlignment$ Center=�6>   $��22��6U   	V_Out :6j   �� 5E      $��13(�6$   $��90��62   ��6=   �� $ Data1�#
 Z $ TwoWay=�$ PropertyChanged=�6�   �� 5F      $��13(�6$   $	��120��62   $ Center=�6>   	I_Out :6^   �� 5G      $��13(�6$   $��90��62   ��6=   �� $ Data21�#
 Z $ TwoWay=�$ PropertyChanged=�5H      �� 5I      $��2��6!   $ 
Horizontal=�6,   
�������5J      �� 5L      $ 
Horizontal=�6!   .+�5M      �� $��12(�6$   $	��120��62   $ Right=�6>   ChargingEnabled :6g   �� 5N      $��13(�6$   $��80��62   ��6=   �� $ Data2�#
 Z $ TwoWay=�$ PropertyChanged=�6�   �� 5O      $��12(�6$   $	��120��62   $ Right=�6>   MaxBatteryVoltage :6i   �� 5P      $��13(�6$   $��80��62   ��6=   �� $ Data22�#
 Z $ TwoWay=�$ PropertyChanged=�5Q      �� 5S      $ 
Horizontal=�6!   .+�5T      �� $��12(�6$   $	��120��62   $ Right=�6>   ShiftPosition :6e   �� 5U      $��13(�6$   $��80��62   ��6=   �� $ Data3�#
 Z $ TwoWay=�$ PropertyChanged=�6�   �� 5V      $��12(�6$   $	��120��62   $ Right=�6>   Target Volt[V] :6f   �� 5W      $��13(�6$   $��80��62   ��6=   �� $ Data23�#
 Z $ TwoWay=�$ PropertyChanged=�5X      �� 5Z      $ 
Horizontal=�6!   .+�5[      �� $��12(�6$   $	��120��62   $ Right=�6>   SystemFault :6c   �� 5\      $��13(�6$   $��80��62   ��6=   �� $ Data4�#
 Z $ TwoWay=�$ PropertyChanged=�6�   �� 5]      $��12(�6$   $	��120��62   $ Right=�6>   Target Current[A] :6i   �� 5^      $��13(�6$   $��80��62   ��6=   �� $ Data24�#
 Z $ TwoWay=�$ PropertyChanged=�5_      �� 5a      $ 
Horizontal=�6!   .+�5b      �� $��12(�6$   $	��120��62   $ Right=�6>   BmsVehicle :6b   �� 5c      $��13(�6$   $��80��62   ��6=   �� $ Data5�#
 Z $ TwoWay=�$ PropertyChanged=�6�   �� 5d      $��12(�6$   $	��120��62   $ Right=�6>   6V   �� 5e      $��13(�6$   $��80��62   ��6=   �� $ Data25�#
 Z $ TwoWay=�$ PropertyChanged=�5f      �� 5h      $ 
Horizontal=�6!   .+�5i      �� $��12(�6$   $	��120��62   $ Right=�6>   NormalStopRequest :6i   �� 5j      $��13(�6$   $��80��62   ��6=   �� $ Data6�#
 Z $ TwoWay=�$ PropertyChanged=�6�   �� 5k      $��12(�6$   $	��120��62   $ Right=�6>   TotalBatteryCapacity :6l   �� 5l      $��13(�6$   $��80��62   ��6=   �� $ Data26�#
 Z $ TwoWay=�$ PropertyChanged=�5m      �� 5o      $ 
Horizontal=�6!   .+�5p      �� $��12(�6$   $	��120��62   $ Right=�6>   6V   �� 5q      $��13(�6$   $��80��62   ��6=   �� $ Data7�#
 Z $ TwoWay=�$ PropertyChanged=�6�   �� 5r      $��12(�6$   $	��120��62   $ Right=�6>   RemainingBatteryEnergy :6n   �� 5s      $��13(�6$   $��80��62   ��6=   �� $ Data27�#
 Z $ TwoWay=�$ PropertyChanged=�5t      5v      5w      �� 5x      -	   5        $��ChargerInfoRect��5x   [   $	 166��6   $	 212��6*   
������ $	��109��6N   $	 100��6�   $	��120��6�   5y      5z      5{      