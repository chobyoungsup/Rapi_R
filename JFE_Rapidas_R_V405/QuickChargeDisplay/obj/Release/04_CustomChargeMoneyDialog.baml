   M S B A M L   `   `   `  ����   QuickChargeDisplay0    *QuickChargeDisplay.CustomChargeMoneyDialog   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml      � Title$  CustomChargeMoneyDialog�� � WindowStyle$
 None=� � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ Manual=� � AllowsTransparency .  � ShowInTaskbar .   � Topmost . ����  �� ImageSource$< 6image\03_충전금액화면\직접입력\베이스.png��.���� $	��472��$	��450��.+��� -    ��Name$ closeButton��$?��9image\03_충전금액화면\직접입력\닫기버튼.png��$��36��$��36��	 �� Left$		 424��

 �� Top$
 12���� -   $ key_1��$4��.image\03_충전금액화면\직접입력\1.png��$	��115��$��74��$	 6��$	
 146���� -   $ key_2��$4��.image\03_충전금액화면\직접입력\2.png��$	��115��$��74��$		 121��$	
 146���� -   $ key_3��$4��.image\03_충전금액화면\직접입력\3.png��$	��115��$��74��$		 236��$	
 146���� -   $ key_4��$4��.image\03_충전금액화면\직접입력\4.png��$	��115��$��74��$	 6��$	
 220���� -   $ key_5��$4��.image\03_충전금액화면\직접입력\5.png��$	��115��$��74��$		 121��$	
 220���� -   $ key_6��$4��.image\03_충전금액화면\직접입력\6.png��$	��115��$��74��$		 236��$	
 220���� -   $ key_7��$4��.image\03_충전금액화면\직접입력\7.png��$	��115��$��74��$	 6��$	
 294���� -	   $ key_8��$4��.image\03_충전금액화면\직접입력\8.png��$	��115��$��74��$		 121��$	
 294���� -
   $ key_9��$4��.image\03_충전금액화면\직접입력\9.png��$	��115��$��74��$		 236��$	
 294���� -   $ key_0��$4��.image\03_충전금액화면\직접입력\0.png��$	��115��$��74��$	 6��$	
 368��$
��Fill=��� -   $ key_00��$5��/image\03_충전금액화면\직접입력\00.png��$	��115��$��74��$		 121��$	
 368��$
��Fill=��� -   $ key_Dot��$<��6image\03_충전금액화면\직접입력\소숫점.png��$	��115��$��74��$		 236��$	
 368��$
��Fill=��� -   $ key_Fix��$9��3image\03_충전금액화면\직접입력\정정.png��$	��115��$��73.75��$		 351��$	
 146���� -   $ 
key_Cancel��$<��6image\03_충전금액화면\직접입력\화살표.png��$	��115��$��73.75��$		 351��$	
 220���� -   $ key_Confirm��$9��3image\03_충전금액화면\직접입력\확인.png��$	��115��$	��149��$		 351��$	
 294���� -    ��Name$ 	txtCharge�� �� TextAlignment$ Right=�$��Center=�
������ 
������ $��font\DroidSans-Bold.ttf)�$
��Bold"�$��50(�
�������$	 6��$
 70��$��442.5��$��60�� �� TextWrapping$
 Wrap=� �� 	MaxLength$	 100�� �� 
IsReadOnly . ����  �� Path$ ChargeValue� �� Mode$ TwoWay=� �� RelativeSource �� *FindAncestor+ �� AncestorType" �