   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  K*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  *    $QuickChargeDisplay.ManualControlPage   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    0local clr-namespace:QuickChargeDisplay    <convert*clr-namespace:QuickChargeDisplay.Converter    -my clr-namespace:QuickChargeDisplay    -     N� Title$  ManualControlPage��$	��800��$	��480�� � 	Resources ;   5QuickChargeDisplay.Converter.DoublePrecisionConverter   doublePrecisionConverter �� 
TargetType  ButtonTextBlock �� Property �� Value  
labelStyle  textBoxStyle��%�  &	        &	      &	 �     &	 �      �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 10pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  � ��$ Center=���  � ��$ Center=��� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �   ���  � ��
 �������  �@��TextAlignment$ Right=���  � ��$ Center=���  �@��
IsReadOnly .  N� 
Background ��  �� ImageSource$8 2image\10_관리자화면\관리자기본배경.gif��.���� .+��� $<��6image\10_관리자화면\아이콘\수동충전_ON.gif�� �� Left$	 323��
 �� Top$ 20��$��50��$��48���� #��[ $��30(�$	 380��$ 26��Manual Charge�� -   	 ��Name$	 logOut��$4��.image\10_관리자화면\아이콘\LOGOUT1.gif��$	 741��$ 19��$��50��$��55���� -   $	 ChargeDetails��$=��7image\10_관리자화면\아이콘\충전내역_OFF.gif��$ 15��$ 98��$��50��$��50���� -   $	 TroubleDetails��$=��7image\10_관리자화면\아이콘\고장내역_OFF.gif��$ 19��$	 197��$��50��$��50���� -   $	 ManualControl��$<��6image\10_관리자화면\아이콘\수동충전_ON.gif��$ 19��$	 298��$��50��$��50���� -   $	 SetupEquipment��$=��7image\10_관리자화면\아이콘\기기설정_OFF.gif��$ 19��$	 401��$��50��$��50���� $ 5��$	 150��#��[ $��16(�$
��Wrap=�$��40��$��64��
 �� TextAlignment$
 Center=�Charge History�� $ 6��$	 248��#��[ $��16(�$
��Wrap=�$��40��$��64��$
 Center=�Fault History�� $ 6��$	 349��#��[ $��16(�$
��Wrap=�$��40��$��64��$
 Center=�Manual Charge�� $ 6��$	 452��#��[ $��16(�$
��Wrap=�$��25��$��64��$
 Center=�Setup�� $	 414��$	 407��$��11.666�� K� ZIndex$ -1�� J� RenderTransformOrigin$ 0.5,0.5&�$��51������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$	 Red�� � Offset$ 0Q�� $	 Red��$ 1Q�� $ White��$ 0.522Q�z�V� .~��� �� ��  �� Angle$	 360Q�T� �� $	 234��$	 406��$��11.666��$ -1��$ 0.5,0.5&�$��51������ $ 0.5,1&�$ 0.5,0&�.��� $	 Red��$ 0Q�� $	 Red��$ 1Q�� $ White��$ 0.522Q�z�V� .~��� �� �� $	 360Q�T� �� -   $��	BtnPraRun��$	 102��$	 362��$	��106��$
��Fill=�$	��137��$H��B/QuickChargeDisplay;component/image/10_관리자화면/PRA_RUN.png�� � DataContext �� �� -   $��BtnPCSReadyON��$	 283��$	 362��$	��106��$M��G/QuickChargeDisplay;component/image/10_관리자화면/PCS_READY_ON.png��$
��Fill=�$	��137�� �� �� -	   $��BtnTsBatteryRun��$	 459��$	 362��$	��106��$N��H/QuickChargeDisplay;component/image/10_관리자화면/BATTERY_START.png��$
��Fill=�$	��137�� �� �� -
   $��BtnTsFinish��$	 639��$	 362��$	��106��$O��I/QuickChargeDisplay;component/image/10_관리자화면/BATTERY_FINISH.png��$
��Fill=�$	��137�� �� �� $	 593��$	 408��$��11.666��$ -1��$ 0.5,0.5&�$��51������ $ 0.5,1&�$ 0.5,0&�.��� $	 Red��$ 0Q�� $	 Red��$ 1Q�� $ White��$ 0.522Q�z�V� .~��� �� �� $	 360Q�T� �� -   $��label1��$ 94��$	 163��$��
BMS Status��$��44��$	��182�����	#FFA7A7A7$��24(�$��Center=�$��Center=�$��RightToLeft=��� -   $��label3�����	#FFA7A7A7$	 447��$	 163��$��
PCS Status��$��24(�$��44��$	��182��$��Center=�$��Center=�$��RightToLeft=��� -   $��label5�����	#FFA7A7A7$ 94��$	 307��$��Current��$��24(�$��41��$��Center=�$	��182��$��RightToLeft=��� -   $��label6�����	#FFA7A7A7$	 447��$	 307��$��Temperature��$��24(�$��41��$��Center=�$	��182��$��RightToLeft=��� -   $��label9�����	#FFA7A7A7$	 447��$	 261��$	��SOC��$��24(�$��40��$��Center=�$	��182��$��RightToLeft=��� -   $��label13�����	#FFA7A7A7$ 94��$	 261��$��Cell Voltage��$��24(�$��40��$��Center=�$	��182��$��RightToLeft=��� -   $��label14�����	#FFA7A7A7$ 94��$	 213��$��Pack Voltage��$��24(�$��42��$��Center=�$	��182��$��RightToLeft=��� -   $��label2�����	#FFA7A7A7$	 447��$	 213��$��ChargeSetPoint��$��RightToLeft=�$��24(�$��42��$��Center=�$	��182���� $��487,12,0,43q�
���   �
�������$��20(�$��
HYHeadLine)� �� TextWrapping$
 Wrap=�$��Center=�$
��Left=�$	��150�� �� TextAlignment$ Center=�{�.  �� 
IsReadOnly . $��1��$
 -206��$	 151��$��44������  �� Path$ 	BMSStatus�   QuickChargeDisplay.QCDV   ManualControl �� Source# Z  �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� $��487,12,0,43q�
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��150��$ Center=�{�.  . $��1��$	 148��$	 151��$��44������ $ 	PCSStatus�# Z $ TwoWay=�$ PropertyChanged=��� $��487,12,0,43q�
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��150��$ Center=�{�.  . $��1��$	 148��$	 295��$��41������ $ AverageTemperature�# Z $ TwoWay=�$ PropertyChanged=��� $��487,12,0,43q�
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��150��$ Center=�{�.  . $��1��$	 148��$	 249��$��40������ $	 SOC�# Z $ TwoWay=�$ PropertyChanged=��� $��487,12,0,43q�
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��150��$ Center=�{�.  . $��1��$
 -205��$	 249��$��40������ $ AverageCellVoltage�# Z $ TwoWay=�$ PropertyChanged=��� $��487,12,0,43q�
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��150��$ Center=�{�.  . $��1��$
 -205��$	 295��$��41������ $ BatteryCurrent�# Z $ TwoWay=�$ PropertyChanged=��� $��487,12,0,43q�
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��150��$ Center=�{�.  . $��1��$
 -204��$	 201��$��41������ $ BatteryVoltage�# Z $ TwoWay=�$ PropertyChanged=��� $��487,12,0,43q�
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��150��$ Center=�{�.  . $��1��$	 148��$	 200��$��42������ $ ChargeVoltageSetPoint�# Z $ TwoWay=�$ PropertyChanged=��� -   $��BtnBmsSleep��$ 94��$	 102��$��46��$	��184��$��	BMS Sleep��$��20(��� -   $��	BtnWakeup��$	 348��$	 102��$��
BMS Wakeup��$��46��$	��184��$��20(��� -   $��BtnReset��$	 601��$	 102��$��	BMS Reset��$��46��$	��184��$��20(�