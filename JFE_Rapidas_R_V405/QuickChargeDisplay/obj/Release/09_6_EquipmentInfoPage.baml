   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  K*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  *    $QuickChargeDisplay.EquipmentInfoPage   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    0local clr-namespace:QuickChargeDisplay    <convert*clr-namespace:QuickChargeDisplay.Converter    -     N� Title$  EquipmentInfoPage��$
��1024��$	��768�� � 	Resources ;   5QuickChargeDisplay.Converter.DoublePrecisionConverter   doublePrecisionConverter �� 
TargetType  ButtonTextBlock �� Property �� Value  
LabelStyle  
blackLabel  textBoxStyle��%E  &	        &	      &	 �     &	 �    &	 4      �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  � ��$ Right=��� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��
 �   ���  � ��
 ���� ��  � ��$ Right=���  � ��$ 3q��� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��
 �   ���  � ��
 �������  �@��TextAlignment$ Right=���  � ��$ Center=���  �@��
IsReadOnly .  N� 
Background ��  �� ImageSource$8 2image\10_관리자화면\관리자기본배경.gif��.���� .+��� $<��6image\10_관리자화면\아이콘\기기정보_ON.gif�� �� Left$	 442��
 �� Top$ 24��$��50��$��48���� #��[ $��30(�$	 498��$ 30��기기정보�� -   	 ��Name$	 logOut��$4��.image\10_관리자화면\아이콘\LOGOUT1.gif��$	 954��$ 24��$��50��$��55���� -   $	 ChargeDetails��$=��7image\10_관리자화면\아이콘\충전내역_OFF.gif��$ 43��$	 107��$��44��$��51���� -   $	 TroubleDetails��$=��7image\10_관리자화면\아이콘\고장내역_OFF.gif��$ 46��$	 199��$��43��$��45���� -   $	 AdjustUnitPrice��$=��7image\10_관리자화면\아이콘\단가조정_OFF.gif��$ 43��$	 286��$��45��$��50���� -   $	 SetupEquipment��$=��7image\10_관리자화면\아이콘\기기설정_OFF.gif��$ 43��$	 374��$��45��$��49���� -   $	 
PowerMeter��$=��7image\10_관리자화면\아이콘\파워메타_OFF.gif��$ 40��$	 465��$��48��$��61���� -   $	 EquipmentInfo��$<��6image\10_관리자화면\아이콘\기기정보_ON.gif��$ 48��$	 569��$��40��$��60���� -	   $	 FactorySetup��$=��7image\10_관리자화면\아이콘\공장설정_OFF.gif��$ 44��$	 679��$��50��$��50���� $ 25��$	 162��#��[ 충전내역�� $ 25��$	 250��#��[ 고장내역�� $ 25��$	 341��#��[ 단가조정�� $ 25��$	 429��#��[ 기기설정�� $ 25��$	 532��#��[ 파워메타�� $ 25��$	 635��#��[ 기기정보�� $ 25��$	 735��#��[ 공장설정�� 
���   �$	 182��$	 130��|�.  $��font\YoonYGO550 TT.ttf)�$��24(�$
��Bold"�
�������$
��42.5��$��Center=�{�. 
 �� 
IsReadOnly
 . $��Center=�$	��180��.��Quick Charger� $	 170��$ 172.5��
�������$
��87.5��$	��814�� � ColumnDefinitions �� $	��120 ��� $
��Auto ��� $	��250 ��� $
��Auto � � RowDefinitions �� $
��Auto ��� $
��Auto �.+��� #��[ $��Right=�$��5q�$��0��$��0��.��	QC Code�� #��[ $��Right=�$��5q�$��0��$��2��.��
QC Version�� #��[ $��Right=�$��5q�$��1��$��0��.��
Zip Code�� #��[ $��Right=�$��5q�$��1��$��2��.��Hardware Version�� #��[ $��0��$��1��$��3q�����  �� Path$ QcCode�   QuickChargeDisplay.QCDV   EquipmentInfo �� Source# Z  �� UpdateSourceTrigger$ PropertyChanged=��� #��[ $��0��$��3��$��3q�$	��120������ $ 	QcVersion�# Z $ PropertyChanged=��� $��1��$��1�� K� ZIndex$ 2�� �� Orientation$ 
Horizontal=�.+��� -
    ��Name$ zipCode1��
������ $ 2��$��3q�$	��100���� -   $ zipCode2��
������ $ 2��$��3q�$	��100���� -   $ zipCode3��
������ $ 2��$��3q�$	��100���� $��1��$��1��$ 1��$ 
Horizontal=�.+��� #��[ $ 1��$��Center=�$��3q�$	��100������ $ ZipCode1�# Z $ PropertyChanged=��� #��[ $ 1��$��Center=�$��3q�$	��100������ $ ZipCode2�# Z $ PropertyChanged=��� #��[ $ 1��$��Center=�$��3q�$	��100������ $ ZipCode3�# Z $ PropertyChanged=��� #��[ $��1��$��3��$��3q�$	��120������ $ HardwareVersion�# Z $ PropertyChanged=��� 
���   �$	 185��$	 315��|�.  $��font\YoonYGO550 TT.ttf)�$��24(�$
��Bold"�
�������$
��42.5��$��Center=�{�. 
 . $��Center=�$	��180��.��B.M.S� $	 170��$	 355��
�������$	��815��$��347.5�� �� $	��200 ��� $	��300 ��� $	��180 ��� $
��Auto � �� $
��Auto ��� $
��Auto ��� $��* �.+��� #��[ $��Right=�$��5q�$��0��$��0��.��VehicleCode�� #��[ $��Right=�$��5q�$��0��$��2��.��BMS Version�� #��[ $��Right=�$��5q�$��1��$��0��.��ManufacturerCode�� #��[ $��Right=�$��5q�$��1��$��2��.��Charging Mode�� #��[ $��0��$��1��$��5q����� $ VehicleCode�# Z $ PropertyChanged=��� #��[ $��0��$��3��$��5q�$	��120������ $ 
BmsVersion�# Z $ PropertyChanged=��� #��[ $��1��$��1��$��5q����� $ ManufacturerCode�# Z $ PropertyChanged=��� #��[ $��1��$��3��$��5q�$	��120������ $ ChargingMode�# Z $ PropertyChanged=�� $��3��$��4�� �� $
��402* ��� $
��404* �.+�� $��Center=�$��10q�
������� �� $
��Auto ��� $��* � �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��#��[ .��	SOC [%]�� $��1��#��[ .��Target Voltage [V]�� $��2��#��[ .��Target Current [A]�� $��3��#��[ .��Target Power [kW]�� $��4��#��[ .��Battery Voltage [V]�� $��5��#��[ .��Battery Current [A]�� #��[ $��0��$��1��$	��150��$��3q����� $ 
SocPercent�# Z $ PropertyChanged=� �� 	Converter# [   �� ConverterParameter$ {0:F2}���� #��[ $��1��$��1��$	��150��$��3q����� $ TargetVoltage�# Z $ PropertyChanged=�# [  $ {0:F2}���� #��[ $��2��$��1��$	��150��$��3q����� $ TargetCurrent�# Z $ PropertyChanged=�# [  $ {0:F2}���� #��[ $��3��$��1��$	��150��$��3q����� $ TargetPower�# Z $ PropertyChanged=�# [  $ {0:F2}���� #��[ $��4��$��1��$	��150��$��3q����� $ BatteryVoltage�# Z $ PropertyChanged=�# [  $ {0:F2}���� #��[ $��5��$��1��$	��150��$��3q����� $ BatteryCurrent�# Z $ PropertyChanged=�# [  $ {0:F2}��� $��1��$��Center=�$��10q�
������� �� $
��Auto ��� $��* � �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��#��[ .��Ready�� $��1��#��[ .��	Warning�� $��2��#��[ .��Fault�� $��3��#��[ .��Charging Relay�� $��4��#��[ .��Charging Condition�� $��5��#��[ .��Charging Finish�� #��[ $��0��$��1��$	��150��$��3q����� $ Ready�# Z $ PropertyChanged=��� #��[ $��1��$��1��$	��150��$��3q����� $ Warning�# Z $ PropertyChanged=��� #��[ $��2��$��1��$	��150��$��3q����� $ Fault�# Z $ PropertyChanged=��� #��[ $��3��$��1��$	��150��$��3q����� $ ChargingRelay�# Z $ PropertyChanged=��� #��[ $��4��$��1��$	��150��$��3q����� $ ChargingCondition�# Z $ PropertyChanged=��� #��[ $��5��$��1��$	��150��$��3q����� $ ChargingFinish�# Z $ PropertyChanged=�