   M S B A M L   `   `   `  ����   QuickChargeDisplay%    QuickChargeDisplay.KeyPadWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  KeyPadWindow�� � 
ResizeMode$ NoResize=� � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterOwner=� � WindowStyle$
 None=� � Topmost . .��� 
���   � � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ closeButton��$��4��$��Right=�$?��9image\03_충전금액화면\직접입력\닫기버튼.png��$��36��$��36��	 �� Left$		 424��

 �� Top$
 12���� -    ��Name$ 	inputText��$��1��$��font\DroidSans-Bold.ttf)�$��40(�
�������$��Center=�$��Right=�$��4��$��60��
���   �����  �� Path$ 
InputValue� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� RelativeSource �� *FindAncestor+ �� AncestorType" ��� -   $ key_1��$��2��$��0��$4��.image\03_충전금액화면\직접입력\1.png��$	��115��$��74���� -   $ key_2��$��2��$��1��$4��.image\03_충전금액화면\직접입력\2.png��$	��115��$��74���� -   $ key_3��$��2��$��2��$4��.image\03_충전금액화면\직접입력\3.png��$	��115��$��74���� -   $ key_4��$��3��$��0��$4��.image\03_충전금액화면\직접입력\4.png��$	��115��$��74���� -   $ key_5��$��3��$��1��$4��.image\03_충전금액화면\직접입력\5.png��$	��115��$��74���� -	   $ key_6��$��3��$��2��$4��.image\03_충전금액화면\직접입력\6.png��$	��115��$��74���� -
   $ key_7��$��4��$��0��$4��.image\03_충전금액화면\직접입력\7.png��$	��115��$��74���� -   $ key_8��$��4��$��1��$4��.image\03_충전금액화면\직접입력\8.png��$	��115��$��74���� -   $ key_9��$��4��$��2��$4��.image\03_충전금액화면\직접입력\9.png��$	��115��$��74���� -   $ key_0��$��5��$��0��$4��.image\03_충전금액화면\직접입력\0.png��$	��115��$��74��$
��Fill=��� -   $ key_00��$��5��$��1��$5��/image\03_충전금액화면\직접입력\00.png��$	��115��$��74��$
��Fill=��� -   $ key_Dot��$��5��$��2��$f��`/QuickChargeDisplay;component/image/03_충전금액화면/충전금액버튼들/취소버튼.png��$	��115��$��75��$
��Fill=��� -   $ key_Fix��$��2��$��3��$9��3image\03_충전금액화면\직접입력\정정.png��$	��115��$��73.75���� -   $ 
key_Cancel��$��3��$��3��$<��6image\03_충전금액화면\직접입력\화살표.png��$	��115��$��73.75���� -   $ key_Confirm��$��4��$��3��$��2��$9��3image\03_충전금액화면\직접입력\확인.png��$	��115��$	��149��