   M S B A M L   `   `   `  ����M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:QuickChargeDisplay.ControlQuickChargeDisplay.Control  5      7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  5      *    $QuickChargeDisplay.ConfirmChargePage   5      R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      :control(clr-namespace:QuickChargeDisplay.Control    5      0local clr-namespace:QuickChargeDisplay    5      -   5          N� Title$  ConfirmChargePage��5      $	��800��6!   $	��480��6-    � 	Resources 5       �� 
TargetType �� Property �� Value��%�  '��       �� 5	   
   " ��6   .�5
      ��  � ��$ font\DroidSans-Bold.ttf)�6+   �� 5       � ��$
 40pt(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��
 �����6+   �� 5       � ��
 ���� 6+   �� 5       � ��
 ���� 6,   �� 5       � ��$ Bottom=�69   �� 5       � ��$ Right=�6;   �� 5       �@��TextAlignment$ Right=�6.   �� 5       � |� .  6*   5      5       N� 
Background 5      �� 5   
    �� ImageSource$ image\06_결재확인.gif��6   .��5      �� .+�5   
   .   (QuickChargeDisplay.Control.AnimatedImage  -   5          Name$ 	BackImage��5       65   �� 5   
   -   5         ��Name$ paymentMoney��5      $��65��6(   	 �� Left$		 426��64   

 �� Top$	
 271��6F   $	��140��6W   $��40(�6�   $y�Hidden=�6�   ��6c   ��  �� Path$ Payment� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   QuickChargeDisplay.QCDV   ConfirmCharge �� Source# Z 6�   �� 5   
   -   5        $ 
chargeWatt��5      $��66��6&   $		 357��62   $	
 396��6D   $	��147��6U   $��40(�6�   ��6a   �� $ 
ChargeWatt�$ TwoWay=�$ PropertyChanged=�# Z 6�   5      5       