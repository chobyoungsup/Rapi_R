   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:QuickChargeDisplay.ControlQuickChargeDisplay.Control  7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  *    $QuickChargeDisplay.ConfirmChargePage   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    :control(clr-namespace:QuickChargeDisplay.Control    0local clr-namespace:QuickChargeDisplay    -     N� Title$  ConfirmChargePage��$	��800��$	��480�� � 	Resources  �� 
TargetType �� Property �� Value��%(  '��       �� " ��.���  � ��$ font\DroidSans-Bold.ttf)���  � ��$
 40pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  � ��
 ���� ��  � ��$ Bottom=���  � ��$ Right=���  �@��TextAlignment$ Right=���  � |� .   N� 
Background ��  �� ImageSource$ image\06_결재확인.gif��.���� .+�.   (QuickChargeDisplay.Control.AnimatedImage  -     Name$ 	BackImage���� -    ��Name$ paymentMoney��$��65��	 �� Left$		 426��

 �� Top$	
 271��$	��140��$��40(�$y�Hidden=�����  �� Path$ Payment� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   QuickChargeDisplay.QCDV   ConfirmCharge �� Source# Z �� -   $ 
chargeWatt��$��66��$		 357��$	
 396��$	��147��$��40(����� $ 
ChargeWatt�$ TwoWay=�$ PropertyChanged=�# Z 