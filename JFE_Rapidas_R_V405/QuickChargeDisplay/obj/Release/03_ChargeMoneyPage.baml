   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:QuickChargeDisplay.ConverterQuickChargeDisplay.Converter  7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  (    "QuickChargeDisplay.ChargeMoneyPage   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    >d2http://schemas.microsoft.com/expression/blend/2008    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    <convert*clr-namespace:QuickChargeDisplay.Converter    0local clr-namespace:QuickChargeDisplay    -     N� Title$  ChargeMoneyPage��$
��1024��$	��768�� N� 
Background ��  �� ImageSource$ image/03_충전금액.gif�� � 	Resources ;   5QuickChargeDisplay.Converter.DoublePrecisionConverter   doublePrecisionConverter:   4QuickChargeDisplay.Converter.CurrentTimeTwoUnitPrice  CurrentTimeTwoUnitPrice5   /QuickChargeDisplay.Converter.UnitPriceConverter  UnitPriceConverter@   :QuickChargeDisplay.Converter.SocPercentToRotationConverter ! SocPercentToRotationConverter6   0QuickChargeDisplay.Converter.SocPercentConverter  SocPercentConverter  defaultButton �� 
TargetType ��Name � MappingMode �� EndPoint �� 
StartPoint	 � Color
 � Offset �� Triggers P� Property P� Value �� 
TargetName �� Property �� Value �� 
TargetType-  'System.Windows.Input.KeyboardNavigation   TabNavigation � RowDefinitions K� ZIndex K� IsItemsHost   TabIndex �� CornerRadius   DirectionalNavigation 	 First ��Name �� Triggers �� Value �� Binding �� RelativeSource �� Mode �� AncestorType  �� Path 
 Second 	 Third��%�  &	        &	      &	 
     &	      &	      &	      '�� �    &	 &    &	 [    &	 Y
              �� " ��.R�� $	��0.5q�.+��� $ BackRect������ $ RelativeToBoundingBox=�$ 0.5,1&�$ 0.5,0&�.��� $
	 Gray��$	
 1.5Q�� $	 White���� $��Center=�$��Center=�$��./font/DroidSans-Bold.ttf)�$
��Bold"� P�  � �� . . ��� $ BackRect�� � �� �� $ RelativeToBoundingBox=�$ 0.5,1&�$ 0.5,0&�.��� $	 	#FF401A95��$
 1Q�� $	 	#FF6639CB��$
 0Q�� $	 	#FF6639AF��$

 0.47Q�� $	 	#FE401A95��$	
 0.5Q��� " ��.���  �@�OverridesDefaultStyle .  ��  �@J�SnapsToDevicePixels .  ��  � �� �� " ��.R�� $ Local=� �� $
��Auto ��� $��* �.+��� $��HeaderPanel��$��0��$ 1��$��0,0,0,1q� . $ 1��
������ �� $��Border��$��1��
������ 
������ $��0q�$ 0��$ Local=�$ 	Contained=�$ 2��._��� $��PART_SelectedContentHost��$��SelectedContent�� P�  � {� .  . ���  � ��
 ���� ��  � ��
 ���� ��  � ��
 ���� ��  � ��
 ���� ��  � ��
 ���� �� " ��.���  � ��$ 0q���  � ��
 ���� ��  � ��$ 0q���  � ��
 ���� ��  � �� �� .R�� .+��� $ chargeMoney��$��-0.5,0,0,-1q�$N��Himage\03_충전금액화면\충전금액버튼들\충전금액활성.png��$	��153��$��74��$
��Fill=�$��Center=�$��Center=��� $ chargeMoney2��$Q��Kimage\03_충전금액화면\충전금액버튼들\충전금액비활성.png��$	��153��$��74��$
��Fill=�$��Center=�$��Center=� �� $
 True�� ��  �� $ FindAncestor=�" ��$  
IsSelected�.a��� $ chargeMoney�� � y�$ Visible=��� $ chargeMoney2�� � y�$ 	Collapsed=��� $ False�� ��  �� $ FindAncestor=�" ��$  
IsSelected�.a��� $ chargeMoney�� � y�$ 	Collapsed=��� $ chargeMoney2�� � y�$ Visible=��� " ��.���  � ��$ 0q���  � ��
 ���� ��  � ��$ 0q���  � ��
 ���� ��  � �� �� .R�� .+��� $ chargePower��$��0,0,0,-1q�$N��Himage\03_충전금액화면\충전금액버튼들\충전전력활성.png��$	��153��$��74��$
��Fill=��� $ chargePower2��$Q��Kimage\03_충전금액화면\충전금액버튼들\충전전력비활성.png��$	��153��$��74��$
��Fill=� �� $
 True�� ��  �� $ FindAncestor=�" ��$  
IsSelected�.a��� $ chargePower�� � y�$ Visible=��� $ chargePower2�� � y�$ 	Collapsed=��� $ False�� ��  �� $ FindAncestor=�" ��$  
IsSelected�.a��� $ chargePower�� � y�$ 	Collapsed=��� $ chargePower2�� � y�$ Visible=��� " ��.���  � ��$ 0q���  � ��
 ���� ��  � ��$ 0q���  � ��
 ���� ��  � �� �� .R�� .+��� $ charge��$��0,0,0,-1q�$K��Eimage\03_충전금액화면\충전금액버튼들\SOC충전활성.png��$	��153��$��74��$
��Fill=��� $ charge2��$N��Himage\03_충전금액화면\충전금액버튼들\SOC충전비활성.png��$	��153��$��74��$
��Fill=� �� $
 True�� ��  �� $ FindAncestor=�" ��$  
IsSelected�.a��� $ charge�� � y�$ Visible=��� $ charge2�� � y�$ 	Collapsed=��� $ False�� ��  �� $ FindAncestor=�" ��$  
IsSelected�.a��� $ charge�� � y�$ 	Collapsed=��� $ charge2�� � y�$ Visible=�.���� .+��� -   $ ChargeDevice��
������ $	 100��$	��197��$	��153��! �� Left$! 62��
" �� Top$	" 417��(   "QuickChargeDisplay.ExtendedBinding  #   Source# �� $  
ChargedSoc�   QuickChargeDisplay.QCDV$   ChargeMoney% �� Source#% Z$ &   Target& �� $  ChargePercent� �� *FindAncestor+" N��� -   ' ��Name$' chargeUnitPrice��$��./font/YoonYGO550 TT.ttf)�$��40(�
�������( �� 
IsReadOnly( . |�.  
������ 
������ $	! 211��$	" 361��$��Center=�) �� TextAlignment$) Right=�$	��133��$��45������ $  CurrentUnitPrice�#% Z$ * �� Mode$* TwoWay=��� -   $��txtChargeAmount��
������ 
������ 
�������( . |�.  $��./font/DroidSans-Bold.ttf)�$��55(�$
��Bold"�$��Center=�$) Right=�$��1��$��73.348��$! 510.949��$" 333.4��$��384.23������ $  ChargeValue�#% Z$ $* TwoWay=�+ �� UpdateSourceTrigger$+ PropertyChanged=��� -   $' txtChargeUnit��$��Right=�$��80.016��
������ 
������ 
�������( . |�.  $��./font/DroidSans-Bold.ttf)�$��30(�$
��Bold"�$��Center=�$
) Left=�$	��원��$��1��$��43.329��$! 891.845��$" 349.965���� -   , ��Name$, chargingTab��|�.  $��313.396��$! 513.436��$" 410.082��$��463.426��.@��� $��금액충전��$	��154��$��74��#��[ - � FocusVisualStyle- �� .��� $��0,0,0,0q�
������ . � ColumnDefinitions. �� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -   / ��Name$/ chargeMoney2000��$��0��$	��0.5q�$	��153��$��74��#���  |�.  .���� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�2,000�� -   $/ chargeMoney3000��$��1��$	��0.5q�$	��153��$��74��#���  |�.  .���� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�3,000�� -	   $/ chargeMoney4000��$��2��$	��0.5q�$	��153��$��74��#���  |�.  .���� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�4,000�� -
   $/ chargeMoney5000��$��0��$	��0.5q�$��1��$	��153��$��74��#���  |�.  .���� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�5,000�� -   $/ chargeMoneyFull��$��1��$��1��$	��0.5q�$	��153��$��74��#���  $��28(�$
��Bold"�|�.  $��	만충전���� -   $/ chargeMoneyCustom��$��1��$��2��$	��0.5q�$	��153��$��74��#���  $��28(�$
��Bold"�|�.  $��직접입력���� $��Center=�$��2��$��3��0 �� Orientation$0 
Horizontal=�.+��� -   $ chargeMoneyCancel��$	��230��$��88��$H��Bimage\03_충전금액화면\충전금액버튼들\취소버튼.png���� -   $ chargeMoneyOk��$	��230��$��88��$H��Bimage\03_충전금액화면\충전금액버튼들\확인버튼.png���� $��전력량충전��$	��154��$��74��#��[ - �� .��� 
������ . �� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -   $/ chargeWatt5kwh��$��0��$	��0.5q�$	��153��$��74��#���  |�.  .���� $0 
Horizontal=�$��Center=�.+��� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�5�� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��25(�$��Bottom=�$��0,0,0,3q�kWh�� -   $/ chargeWatt10kwh��$��1��$	��0.5q�$	��153��$��74��#���  |�.  .���� $0 
Horizontal=�$��Center=�.+��� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�10�� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��25(�$��Bottom=�$��0,0,0,3q�kWh�� -   $/ chargeWatt15kwh��$��2��$	��0.5q�$	��153��$��74��#���  |�.  .���� $0 
Horizontal=�$��Center=�.+��� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�15�� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��25(�$��Bottom=�$��0,0,0,3q�kWh�� -   $/ chargeWatt20kwh��$��0��$	��0.5q�$��1��$	��153��$��74��#���  |�.  .���� $0 
Horizontal=�$��Center=�.+��� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�20�� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��25(�$��Bottom=�$��0,0,0,3q�kWh�� -   $/ chargeWattFull��$��1��$	��0.5q�$��1��$	��153��$��74��#���  $��28(�|�.  $��	만충전���� -   $/ chargeWattCustom��$��2��$	��0.5q�$��1��$	��153��$��74��#���  $��28(�|�.  $��직접입력���� $��Center=�$��2��$��3��$0 
Horizontal=�.+��� -   $ chargeWattCancel��$	��230��$��88��$H��Bimage\03_충전금액화면\충전금액버튼들\취소버튼.png���� -   $ chargeWattOk��$	��230��$��88��$H��Bimage\03_충전금액화면\충전금액버튼들\확인버튼.png���� $��	chargeSoc��$	��154��$��74��#��[ - �� .��� 
������ . �� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -   $/ chargeSoc50��$��0��$	��0.5q�$	��153��$��74��#���  |�.  .���� $0 
Horizontal=�$��Center=�.+��� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�50�� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��25(�$��Bottom=�$��0,0,0,3q�%�� -   $/ chargeSoc60��$��1��$	��0.5q�$	��153��$��74��#���  |�.  .���� $0 
Horizontal=�$��Center=�.+��� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�60�� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��25(�$��Bottom=�$��0,0,0,3q�%�� -   $/ chargeSoc70��$��2��$	��0.5q�$	��153��$��74��#���  |�.  .���� $0 
Horizontal=�$��Center=�.+��� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�70�� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��25(�$��Bottom=�$��0,0,0,3q�%�� -   $/ chargeSoc80��$��0��$	��0.5q�$��1��$	��153��$��74��#���  |�.  .���� $0 
Horizontal=�$��Center=�.+��� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��40(�$��Bottom=�80�� $��./font/DroidSans-Bold.ttf)�$
��Bold"�$��25(�$��Bottom=�$��0,0,0,3q�%�� -   $/ chargeSocFull��$��1��$	��0.5q�$��1��$	��153��$��74��#���  $��28(�|�.  $��	만충전���� -   $/ chargeSocCustom��$��2��$	��0.5q�$��1��$	��153��$��74��#���  $��28(�|�.  $��직접입력���� $��Center=�$��2��$��3��$0 
Horizontal=�.+��� -   $ chargeSocCancel��$	��230��$��88��$H��Bimage\03_충전금액화면\충전금액버튼들\취소버튼.png���� -   $ chargeSocOk��$	��230��$��88��$H��Bimage\03_충전금액화면\충전금액버튼들\확인버튼.png���� -   $��BmsInfo��$��BMS Info��
�������
����=H�$��15(�$
! 16.5��$" 603.597��$	��155��$	��451��y��� $  Visible�1   BmsInfo#% Z1 $* TwoWay=�$+ PropertyChanged=�.���� $0 Vertical=�$	��109��$	��408��.+��� $0 
Horizontal=�.+��� $��13(�$	��120��2 �� TextAlignment$2 Right=�
전압 :�� $��13(�$��80������ $  Data1�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $��13(�$	��120��$2 Right=�
전류 :�� $��13(�$��80������ $  Data21�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $��2��$0 
Horizontal=�
��������� $0 
Horizontal=�.+��� $��12(�$	��120��$2 Right=�	Ready :�� $��13(�$��80������ $  Data2�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $��12(�$	��120��$2 Right=�
SOC[%] :�� $��13(�$��80������ $  Data22�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $0 
Horizontal=�.+��� $��12(�$	��120��$2 Right=�	Warning :�� $��13(�$��80������ $  Data3�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $��12(�$	��120��$2 Right=�Target Volt[V] :�� $��13(�$��80������ $  Data23�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $0 
Horizontal=�.+��� $��12(�$	��120��$2 Right=�	Fault :�� $��13(�$��80������ $  Data4�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $��12(�$	��120��$2 Right=�Target Current[A] :�� $��13(�$��80������ $  Data24�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $0 
Horizontal=�.+��� $��12(�$	��120��$2 Right=�Charging Relay :�� $��13(�$��80������ $  Data5�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $��12(�$	��120��$2 Right=�Target Power[KW] :�� $��13(�$��80������ $  Data25�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $0 
Horizontal=�.+��� $��12(�$	��120��$2 Right=�Charging Condition :�� $��13(�$��80������ $  Data6�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $��12(�$	��120��$2 Right=�Battery Voltage[V] :�� $��13(�$��80������ $  Data26�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $0 
Horizontal=�.+��� $��12(�$	��120��$2 Right=�Charging Finish :�� $��13(�$��80������ $  Data7�#% Z1 $* TwoWay=�$+ PropertyChanged=��� $��12(�$	��120��$2 Right=�Battery Current[A] :�� $��13(�$��80������ $  Data27�#% Z1 $* TwoWay=�$+ PropertyChanged=�