   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  %    QuickChargeDisplay.ChargingPage   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    <convert*clr-namespace:QuickChargeDisplay.Converter    0local clr-namespace:QuickChargeDisplay    -     N� Title$  ChargingPage��$	��800��$	��480�� � 	Resources 6   0QuickChargeDisplay.Converter.SocPercentConverter   SocPercentConverter;   5QuickChargeDisplay.Converter.DoublePrecisionConverter  doublePrecisionConverter �� 
TargetType �� Property �� Value��%F  &	        &	      '�� 
         �� " ��.���  � ��$ font\DroidSans-Bold.ttf)���  � ��$
 40pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  � ��
 ���� ��  � ��$ Bottom=���  � ��$ Right=���  �@��TextAlignment$ Right=���  � |� .   N� 
Background ��  �� ImageSource$/ )image\04_충전중화면\04_충전중.jpg��.��� .+�(   "QuickChargeDisplay.ExtendedBinding     Source ��  �� Path$ ChargingSoc�   QuickChargeDisplay.QCDV	   Charging
 �� Source#
 Z	    Target �� $ 
SocPercent� �� RelativeSource �� *FindAncestor+ �� AncestorType" N��� .+��� $E��?image\04_충전중화면\배터리화면\자동차아이콘.png��$
��Fill=�$	��120��$	��121�� �� Left$	 166��
 �� Top$	 201�� K� ZIndex$ 3���� -    ��Name$ remainChargeMinute�� �� TextAlignment$ Right=�$	��136��$	 544��$	 172��$��66.25��$��40(����� $ 
RemainTime�#
 Z	  �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� -   $ chargingMoney��$ Right=�$	��136��$ 24��$	 291��$��60��$��40(�$y�Hidden=����� $ ChargingMoney�#
 Z	 $ TwoWay=�$ PropertyChanged=��� -   $ chargingCurrent��$ Right=�$	��136��$	 544��$	 243��$��66.25��$��40(����� $ ChargingCurrent�#
 Z	 $ TwoWay=�$ PropertyChanged=��� -   $ chargingKwh��$ Right=�$	��136��$	 544��$	 312��$��66.25��$��40(����� $ ChargingWatt�#
 Z	 $ TwoWay=�$ PropertyChanged=� �� 	Converter# [  �� ConverterParameter$ {0:F1}���� -   $ chargingSoc��$ Right=�$	��136��$	 544��$	 384��$��66.25��$��40(����� $ ChargingSoc�#
 Z	 $ TwoWay=�$ PropertyChanged=�# [ $ {0:F1}���� -    ��Name$ abortCharge��$5��/image\04_충전중화면\충전중지버튼.png��$	��265��$��96��$	 100��$	 354���� -   $��BmsInfo��$��Charger Info��
�������$��15(�$ 24��$	 155��$��52��$	��429��y��� $ Visible�   BmsInfo#
 Z $ TwoWay=�$ PropertyChanged=�.����  �� Orientation$ Vertical=�$	��109��$	��408��.+��� $ 
Horizontal=�.+��� $��13(�$	��120�� �� TextAlignment$ Center=�$��22��	V_Out :�� $��13(�$��90������ $ Data1�#
 Z $ TwoWay=�$ PropertyChanged=��� $��13(�$	��120��$ Center=�	I_Out :�� $��13(�$��90������ $ Data21�#
 Z $ TwoWay=�$ PropertyChanged=��� $��2��$ 
Horizontal=�
��������� $ 
Horizontal=�.+��� $��12(�$	��120��$ Right=�ChargingEnabled :�� $��13(�$��80������ $ Data2�#
 Z $ TwoWay=�$ PropertyChanged=��� $��12(�$	��120��$ Right=�MaxBatteryVoltage :�� $��13(�$��80������ $ Data22�#
 Z $ TwoWay=�$ PropertyChanged=��� $ 
Horizontal=�.+��� $��12(�$	��120��$ Right=�ShiftPosition :�� $��13(�$��80������ $ Data3�#
 Z $ TwoWay=�$ PropertyChanged=��� $��12(�$	��120��$ Right=�Target Volt[V] :�� $��13(�$��80������ $ Data23�#
 Z $ TwoWay=�$ PropertyChanged=��� $ 
Horizontal=�.+��� $��12(�$	��120��$ Right=�SystemFault :�� $��13(�$��80������ $ Data4�#
 Z $ TwoWay=�$ PropertyChanged=��� $��12(�$	��120��$ Right=�Target Current[A] :�� $��13(�$��80������ $ Data24�#
 Z $ TwoWay=�$ PropertyChanged=��� $ 
Horizontal=�.+��� $��12(�$	��120��$ Right=�BmsVehicle :�� $��13(�$��80������ $ Data5�#
 Z $ TwoWay=�$ PropertyChanged=��� $��12(�$	��120��$ Right=��� $��13(�$��80������ $ Data25�#
 Z $ TwoWay=�$ PropertyChanged=��� $ 
Horizontal=�.+��� $��12(�$	��120��$ Right=�NormalStopRequest :�� $��13(�$��80������ $ Data6�#
 Z $ TwoWay=�$ PropertyChanged=��� $��12(�$	��120��$ Right=�TotalBatteryCapacity :�� $��13(�$��80������ $ Data26�#
 Z $ TwoWay=�$ PropertyChanged=��� $ 
Horizontal=�.+��� $��12(�$	��120��$ Right=��� $��13(�$��80������ $ Data7�#
 Z $ TwoWay=�$ PropertyChanged=��� $��12(�$	��120��$ Right=�RemainingBatteryEnergy :�� $��13(�$��80������ $ Data27�#
 Z $ TwoWay=�$ PropertyChanged=��� -	   $��ChargerInfoRect��$	 166��$	 212��
������ $	��109��$	 100��$	��120��