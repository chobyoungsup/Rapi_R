   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  -    'QuickChargeDisplay.CompulsionChargePage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:QuickChargeDisplay     -     N� Title$  CompulsionChargePage�� N� 
Background ��  �� ImageSource$8 2image\10_관리자화면\관리자기본배경.png��.���� -    ��Name$ 
Compulsion��.+��� -    ��Name$ logOut��$'��!image/20191018_Add/back_12955.png�� �� Left$	 719��
 �� Top$	 385��$��50��$��59.47���� $ 332.769��$	 387��$��11.666�� K� ZIndex$ -1�� J� RenderTransformOrigin$ 0.5,0.5&�$��51������ 	 �� EndPoint$	 0.5,1&�
 �� 
StartPoint$
 0.5,0&�.���  � Color$	 Red�� � Offset$ 0Q�� $	 Red��$ 1Q�� $ White��$ 0.522Q�z�V� .~��� �� ��  �� Angle$	 360Q�T� �� $ 152.769��$	 386��$��11.666��$ -1��$ 0.5,0.5&�$��51������ $	 0.5,1&�$
 0.5,0&�.��� $	 Red��$ 0Q�� $	 Red��$ 1Q�� $ White��$ 0.522Q�z�V� .~��� �� �� $	 360Q�T� �� -   $ 	BtnPraRun��$ 20.769��$	 342��$	��106��$
��Fill=�$	��137��$H��B/QuickChargeDisplay;component/image/10_관리자화면/PRA_RUN.png�� � DataContext �� �� -   $ BtnPCSReadyON��$ 201.769��$	 342��$	��106��$M��G/QuickChargeDisplay;component/image/10_관리자화면/PCS_READY_ON.png��$
��Fill=�$	��137�� �� �� -   $ BtnTsBatteryRun��$ 377.769��$	 342��$	��106��$N��H/QuickChargeDisplay;component/image/10_관리자화면/BATTERY_START.png��$
��Fill=�$	��137�� �� �� -   $ BtnTsFinish��$ 557.769��$	 342��$	��106��$O��I/QuickChargeDisplay;component/image/10_관리자화면/BATTERY_FINISH.png��$
��Fill=�$	��137�� �� �� $ 511.769��$	 388��$��11.666��$ -1��$ 0.5,0.5&�$��51������ $	 0.5,1&�$
 0.5,0&�.��� $	 Red��$ 0Q�� $	 Red��$ 1Q�� $ White��$ 0.522Q�z�V� .~��� �� �� $	 360Q�T� �� -    ��Name$ label1��$ 12.769��$ 148.176��$��
BMS Status��$��44��$	��182�����	#FFA7A7A7$��24(�$��Center=�$��Center=�$��RightToLeft=��� -	   $ label3�����	#FFA7A7A7$	 391��$ 148.176��$��
PCS Status��$��24(�$��44��$	��182��$��Center=�$��Center=�$��RightToLeft=��� -
   $ label5�����	#FFA7A7A7$ 12.769��$ 292.176��$��Current��$��24(�$��41��$��Center=�$	��182��$��RightToLeft=��� -   $ label6�����	#FFA7A7A7$	 391��$ 292.176��$��Temperature��$��24(�$��41��$��Center=�$	��182��$��RightToLeft=��� -   $ label9�����	#FFA7A7A7$	 391��$ 246.176��$	��SOC��$��24(�$��40��$��Center=�$	��182��$��RightToLeft=��� -   $ label13�����	#FFA7A7A7$ 12.769��$ 246.176��$��Cell Voltage��$��24(�$��40��$��Center=�$	��182��$��RightToLeft=��� -   $ label14�����	#FFA7A7A7$ 12.769��$ 198.176��$��Pack Voltage��$��24(�$��42��$��Center=�$	��182��$��RightToLeft=��� -   $ label2��$	��182��$��Center=�$ 198.176��$	 391��$��42��$��24(�$��RightToLeft=�$��ChargeSetPoint�����	#FFA7A7A7�� 
���   �
�������$��17(�$��
HYHeadLine)� �� TextWrapping$
 Wrap=�$��Center=�$
��Left=�$	��185�� �� TextAlignment$ Center=�{�.  �� 
IsReadOnly . $ 199.769��$ 148.176��$��44������ *	BMSStatus+ �� Mode$ TwoWay=�   QuickChargeDisplay.QCDV   ManualControl �� Source# Z  �� UpdateSourceTrigger$ PropertyChanged=��� 
���   �
�������$��17(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��201��$ Center=�{�.  . $	 579��$ 148.176��$��44������ *	PCSStatus+$ TwoWay=�# Z $ PropertyChanged=��� 
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��201��$ Center=�{�.  . $	 579��$	 291��$��41������ *AverageTemperature+$ TwoWay=�# Z $ PropertyChanged=��� 
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��201��$ Center=�{�.  . $	 579��$ 246.176��$��40������ *SOC+$ TwoWay=�# Z $ PropertyChanged=��� 
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��184��$ Center=�{�.  . $ 200.769��$ 246.176��$��40������ *AverageCellVoltage+$ TwoWay=�# Z $ PropertyChanged=��� 
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��184��$ Center=�{�.  . $ 200.769��$ 292.176��$��41������ *BatteryCurrent+$ TwoWay=�# Z $ PropertyChanged=��� 
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��185��$ Center=�{�.  . $ 200.292��$ 198.176��$��41������ *BatteryVoltage+$ TwoWay=�# Z $ PropertyChanged=��� 
���   �
�������$��20(�$��
HYHeadLine)�$
 Wrap=�$��Center=�$
��Left=�$	��201��$ Center=�{�.  . $	 579��$ 197.176��$��42������ *ChargeVoltageSetPoint+$ TwoWay=�# Z $ PropertyChanged=��� -    ��Name$ BtnBmsSleep��$ 39��$ 87.176��$��46��$	��184��$��	BMS Sleep��$��20(��� -   $ 	BtnWakeup��$	 314��$ 87.176��$��
BMS Wakeup��$��46��$	��184��$��20(��� -   $ BtnReset��$	 570��$ 87.176��$��	PCS Reset��$��46��$	��184��$��20(��� -   $��CahrgeHistory��$	 241��$ 0��$��80��$9��3image\10_관리자화면\아이콘/충전이력.png��$��79���� -   $��TroubleHistory��$	 318��$ 0��$��80��$9��3image\10_관리자화면\아이콘/에러이력.png��$��79���� -   $��PassWord��$	 397��$ 0��$��80��$9��3image\10_관리자화면\아이콘/패스워드.png��$��79���� -   $��LogSampling��$	 720��$ 1��$��80��$9��3image\10_관리자화면\아이콘/로그추출.png��$��79���� -   $��SetupMaxium��$	 562��$ 1��$��78��$?��9image\10_관리자화면\아이콘/최대전류설정.png��$��77���� -   $��TroubleDetails��$ 0��$ 1��$��80��$1��+image/RAPIDAS-R_Monitor/07/관리설정.png��$��78���� -   $��SetupEquipment��$	 478��$ 0��$��80��$7��1image/RAPIDAS-R_Monitor/07/수전전력설정.png��$��82���� -   $��AdjustUnitPrice��$ 79��$ 0��$��80��$ 0.5,0.5&�$1��+image/RAPIDAS-R_Monitor/07/일시정지.png��$��82���� -   $��
PowerMeter��$	 161��$ 0��$��80��$9��3image\10_관리자화면\아이콘/언어설정.png��$��83���� -   $��Langanglab1��$ 0��$ 54��$��管理設定��$��27��$��78�����	#FFF2E9E9 � ForceCursor .  $��Stretch=�$��Center=�$��Bottom=��� -   $��Langanglab2��$	 720��$ 55��$��	Log抽出�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -   $��Langanglab3��$ 79��$ 54��$��時間の設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -   $��Langanglab4��$	 161��$ 54��$��言語設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -    $��Langanglab5��$	 242��$ 54��$��充電履歴�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -!   $��Langanglab6��$	 319��$ 53��$��エラー履歴�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -"   $��Langanglab7��$	 398��$ 54��$��パスワード�� .  ���	#FFF2E9E9$��27��$��Center=�$��78��$��Bottom=��� -#   $��Langanglab8��$	 478��$ 54��$��受電電力設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��81���� -$   $��Langanglab9��$	 562��$ 54��$��電流設定�� .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��78���� -%   $��Langanglab10��$	 640��$ 55��$��Manual�� .  ���	#FFF2E9E9$��27��$��Center=�$��Bottom=�$��78���� -&   $��Imgmanu��$	 562��$ 1��$��81��$H��B/QuickChargeDisplay;component/image/10_관리자화면/Imgmanu.png��$	��238��$	��Top=� �� StretchDirection$
 Both=�