   M S B A M L   `   `   `  ����M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  5      K*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  5      7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  5      *    $QuickChargeDisplay.ManualControlPage   5      R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      0local clr-namespace:QuickChargeDisplay    5      <convert*clr-namespace:QuickChargeDisplay.Converter    5      -my clr-namespace:QuickChargeDisplay    5      -   5          N� Title$  ManualControlPage��5      $	��800��6!   $	��480��6-    � 	Resources 5	      ;   5QuickChargeDisplay.Converter.DoublePrecisionConverter   doublePrecisionConverter �� 
TargetType  ButtonTextBlock �� Property �� Value  
labelStyle  textBoxStyle��%H  &	        &	      &	 ^    &	 �      5   
   6+   �� 5   
   " ��6   .�5      ��  � ��$ font\YoonYGO550 TT.ttf)�6+   �� 5       � ��$
 15pt(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��
 �����6+   �� 5       � ��
 ���� 6+   �� 5       �@��TextAlignment$ Right=�6.   �� 5       � |� .  6*   5      �� 5   
   " ��6   .�5      ��  � ��$ font\YoonYGO550 TT.ttf)�6+   �� 5       � ��$
 10pt(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��
 �����6+   �� 5       � ��
 ���� 6+   �� 5       � ��$ Center=�69   �� 5       � ��$ Center=�6;   5      �� 5!   
   " ��6   .�5"      ��  � ��$ font\YoonYGO550 TT.ttf)�6+   �� 5#       � ��$
 15pt(�6)   �� 5$       � ��$
 Bold"�6+   �� 5%       � ��
 �   �6+   �� 5&       � ��
 �����6+   �� 5'       �@��TextAlignment$ Right=�6.   �� 5(       � ��$ Center=�69   �� 5)       �@��
IsReadOnly . 6+   5*      5,       N� 
Background 5.      �� 5/   
    �� ImageSource$8 2image\10_관리자화면\관리자기본배경.gif��6   .��52      �� .+�54   
   �� $<��6image\10_관리자화면\아이콘\수동충전_ON.gif��6    �� Left$	 323��68   
 �� Top$ 20��6J   $��50��6Z   $��48��6e   �� 55   
   #��[ 6   $��30(�6=   $	 380��6K   $ 26��6]   Manual Charge6|   �� 58   
   -   5        	 ��Name$	 logOut��58      $4��.image\10_관리자화면\아이콘\LOGOUT1.gif��6=   $	 741��6e   $ 19��6w   $��50��6�   $��55��6�   �� 5;   
   -   5        $	 ChargeDetails��5;      $=��7image\10_관리자화면\아이콘\충전내역_OFF.gif��6O   $ 15��6x   $ 98��6�   $��50��6�   $��50��6�   �� 5<   
   -   5        $	 TroubleDetails��5<      $=��7image\10_관리자화면\아이콘\고장내역_OFF.gif��6O   $ 19��6x   $	 197��6�   $��50��6�   $��50��6�   �� 5=   
   -   5        $	 ManualControl��5=      $<��6image\10_관리자화면\아이콘\수동충전_ON.gif��6Q   $ 19��6y   $	 298��6�   $��50��6�   $��50��6�   �� 5>   
   -   5        $	 SetupEquipment��5>      $=��7image\10_관리자화면\아이콘\기기설정_OFF.gif��6O   $ 19��6x   $	 401��6�   $��50��6�   $��50��6�   �� 5?   
   $ 5��6   $	 150��6$   #��[ 65   $��16(�6^   $
��Wrap=�6l   $��40��6�   $��64��6�   
 �� TextAlignment$
 Center=�6�   Charge History6�   �� 5@   
   $ 6��6   $	 248��6$   #��[ 65   $��16(�6^   $
��Wrap=�6l   $��40��6�   $��64��6�   $
 Center=�6�   Fault History6�   �� 5A   
   $ 6��6   $	 349��6$   #��[ 65   $��16(�6^   $
��Wrap=�6l   $��40��6�   $��64��6�   $
 Center=�6�   Manual Charge6�   �� 5B   
   $ 6��6   $	 452��6$   #��[ 65   $��16(�6^   $
��Wrap=�6l   $��25��6�   $��64��6�   $
 Center=�6�   Setup6�   �� 5D   
   $	 414��6   $	 407��6&   $��11.666��67    K� ZIndex$ -1��6G    J� RenderTransformOrigin$ 0.5,0.5&�6Y   $��51��6y   ��5E      �� 5F       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5G      �  � Color$	 Red��6#    � Offset$ 0Q�6/   � 5H      $	 Red��6#   $ 1Q�6/   � 5I      $ White��6#   $ 0.522Q�61   5J      z�5L      V� 5M      .~�5N      �� �� 5O      �� 5P       �� Angle$	 360Q�6&   T� 5Q      5R      5T      �� 5U   
   $	 234��6   $	 406��6&   $��11.666��67   $ -1��6G   $ 0.5,0.5&�6Y   $��51��6y   ��5V      �� 5W      $ 0.5,1&�6&   $ 0.5,0&�67   .��5X      � $	 Red��6#   $ 0Q�6/   � 5Y      $	 Red��6#   $ 1Q�6/   � 5Z      $ White��6#   $ 0.522Q�61   5[      z�5]      V� 5^      .~�5_      �� �� 5`      �� 5a      $	 360Q�6&   T� 5b      5c      5e      �� 5f   
   -   5        $��	BtnPraRun��5f   @   $	 102��6   $	 362��6"   $	��106��63   $
��Fill=�6Q   $	��137��6`   $H��B/QuickChargeDisplay;component/image/10_관리자화면/PRA_RUN.png��6�    � DataContext 6l   �� 6�   �� 5g   
   -   5        $��BtnPCSReadyON��5g   X   $	 283��6   $	 362��6"   $	��106��6K   $M��G/QuickChargeDisplay;component/image/10_관리자화면/PCS_READY_ON.png��6m   $
��Fill=�6�   $	��137��6�    63   �� 6�   �� 5h   
   -	   5        $��BtnTsBatteryRun��5h   X   $	 459��6   $	 362��6"   $	��106��6K   $N��H/QuickChargeDisplay;component/image/10_관리자화면/BATTERY_START.png��6o   $
��Fill=�6�   $	��137��6�    63   �� 6�   �� 5i   
   -
   5        $��BtnTsFinish��5i   X   $	 639��6   $	 362��6"   $	��106��6K   $O��I/QuickChargeDisplay;component/image/10_관리자화면/BATTERY_FINISH.png��6k   $
��Fill=�6�   $	��137��6�    63   �� 6�   �� 5j   
   $	 593��6   $	 408��6&   $��11.666��67   $ -1��6G   $ 0.5,0.5&�6Y   $��51��6y   ��5k      �� 5l      $ 0.5,1&�6&   $ 0.5,0&�67   .��5m      � $	 Red��6#   $ 0Q�6/   � 5n      $	 Red��6#   $ 1Q�6/   � 5o      $ White��6#   $ 0.522Q�61   5p      z�5r      V� 5s      .~�5t      �� �� 5u      �� 5v      $	 360Q�6&   T� 5w      5x      5z      �� 5{   
   -   5        $��label1��5{   S   $ 94��6   $	 163��6!   $��
BMS Status��62   $��44��6G   $	��182��6a   ���	#FFA7A7A7$��24(�6�   $��Center=�6�   $��Center=�6�   $��RightToLeft=�6�   6�   �� 5|   
   -   5        $��label3��5|   y   ���	#FFA7A7A7$	 447��6'   $	 163��69   $��
PCS Status��6J   $��24(�6_   $��44��6m   $	��182��6�   $��Center=�6�   $��Center=�6�   $��RightToLeft=�6�   �� 5}   
   -   5        $��label5��5}   u   ���	#FFA7A7A7$ 94��6'   $	 307��68   $��Current��6I   $��24(�6[   $��41��6i   $��Center=�6�   $	��182��6�   $��RightToLeft=�6�   �� 5~   
   -   5        $��label6��5~   z   ���	#FFA7A7A7$	 447��6'   $	 307��69   $��Temperature��6J   $��24(�6`   $��41��6n   $��Center=�6�   $	��182��6�   $��RightToLeft=�6�   �� 5   
   -   5        $��label9��5   r   ���	#FFA7A7A7$	 447��6'   $	 261��69   $	��SOC��6J   $��24(�6X   $��40��6f   $��Center=�6�   $	��182��6�   $��RightToLeft=�6�   �� 5�   
   -   5        $��label13��5�   z   ���	#FFA7A7A7$ 94��6'   $	 261��68   $��Cell Voltage��6I   $��24(�6`   $��40��6n   $��Center=�6�   $	��182��6�   $��RightToLeft=�6�   �� 5�   
   -   5        $��label14��5�   z   ���	#FFA7A7A7$ 94��6'   $	 213��68   $��Pack Voltage��6I   $��24(�6`   $��42��6n   $��Center=�6�   $	��182��6�   $��RightToLeft=�6�   �� 5�   
   -   5        $��label2��5�   �   ���	#FFA7A7A7$	 447��6'   $	 213��69   $��ChargeSetPoint��6J   $��RightToLeft=�6c   $��24(�6   $��42��6�   $��Center=�6�   $	��182��6�   �� 5�   
   $��487,12,0,43q�6�   
���   �
�������$��20(�6�   $��
HYHeadLine)�6�    �� TextWrapping$
 Wrap=�6�   $��Center=�6  $
��Left=�6.  $	��150��6I   �� TextAlignment$ Center=�6U  {�.  �� 
IsReadOnly . $��1��6�  $
 -206��6�  $	 151��6�  $��44��6�  ��6   ��  �� Path$ 	BMSStatus�   QuickChargeDisplay.QCDV   ManualControl �� Source# Z  �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�  �� 5�   
   $��487,12,0,43q�6�   
���   �
�������$��20(�6�   $��
HYHeadLine)�6�   $
 Wrap=�6�   $��Center=�6  $
��Left=�6.  $	��150��6I  $ Center=�6U  {�.  . $��1��6�  $	 148��6�  $	 151��6�  $��44��6�  ��6   �� $ 	PCSStatus�# Z $ TwoWay=�$ PropertyChanged=�6�  �� 5�   
   $��487,12,0,43q�6�   
���   �
�������$��20(�6�   $��
HYHeadLine)�6�   $
 Wrap=�6  $��Center=�6  $
��Left=�67  $	��150��6R  $ Center=�6^  {�.  . $��1��6�  $	 148��6�  $	 295��6�  $��41��6�  ��6   �� $ AverageTemperature�# Z $ TwoWay=�$ PropertyChanged=�6�  �� 5�   
   $��487,12,0,43q�6�   
���   �
�������$��20(�6�   $��
HYHeadLine)�6�   $
 Wrap=�6�   $��Center=�6  $
��Left=�6(  $	��150��6C  $ Center=�6O  {�.  . $��1��6�  $	 148��6�  $	 249��6�  $��40��6�  ��6   �� $	 SOC�# Z $ TwoWay=�$ PropertyChanged=�6�  �� 5�   
   $��487,12,0,43q�6�   
���   �
�������$��20(�6�   $��
HYHeadLine)�6�   $
 Wrap=�6  $��Center=�6  $
��Left=�67  $	��150��6R  $ Center=�6^  {�.  . $��1��6�  $
 -205��6�  $	 249��6�  $��40��6�  ��6   �� $ AverageCellVoltage�# Z $ TwoWay=�$ PropertyChanged=�6�  �� 5�   
   $��487,12,0,43q�6�   
���   �
�������$��20(�6�   $��
HYHeadLine)�6�   $
 Wrap=�6�   $��Center=�6  $
��Left=�63  $	��150��6N  $ Center=�6Z  {�.  . $��1��6�  $
 -205��6�  $	 295��6�  $��41��6�  ��6   �� $ BatteryCurrent�# Z $ TwoWay=�$ PropertyChanged=�6�  �� 5�   
   $��487,12,0,43q�6�   
���   �
�������$��20(�6�   $��
HYHeadLine)�6�   $
 Wrap=�6�   $��Center=�6  $
��Left=�63  $	��150��6N  $ Center=�6Z  {�.  . $��1��6�  $
 -204��6�  $	 201��6�  $��41��6�  ��6   �� $ BatteryVoltage�# Z $ TwoWay=�$ PropertyChanged=�6�  �� 5�   
   $��487,12,0,43q�6�   
���   �
�������$��20(�6�   $��
HYHeadLine)�6�   $
 Wrap=�6  $��Center=�6  $
��Left=�6:  $	��150��6U  $ Center=�6a  {�.  . $��1��6�  $	 148��6�  $	 200��6�  $��42��6�  ��6   �� $ ChargeVoltageSetPoint�# Z $ TwoWay=�$ PropertyChanged=�6�  �� 5�   
   -   5        $��BtnBmsSleep��5�   ?   $ 94��6   $	 102��6"   $��46��63   $	��184��6R   $��	BMS Sleep��6^   $��20(�6r   6�   �� 5�   
   -   5        $��	BtnWakeup��5�   U   $	 348��6   $	 102��6#   $��
BMS Wakeup��64   $��46��6I   $	��184��6f   $��20(�6r   6�   �� 5�   
   -   5        $��BtnReset��5�   T   $	 601��6   $	 102��6#   $��	BMS Reset��64   $��46��6H   $	��184��6d   $��20(�6p   6~   5�      5�      