   M S B A M L   `   `   `  ���� M  IQuickChargeDisplay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:QuickChargeDisplayQuickChargeDisplay  ,    &QuickChargeDisplay.ManualControlWindow   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    0local clr-namespace:QuickChargeDisplay      � Title$  ManualControlWindow�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=� � SizeToContent$ WidthAndHeight=� � Topmost .  � 	Resources  �� 
TargetType   textBlockStyle �� Property �� Value  buttonStyle  textBoxStyle  mcCheck	 �� 
TargetType
 �� 
Background �� Bullet ��Name �� Triggers P� Property P� Value �� 
TargetName��%�  &	        &	 �     &	 }    &	 O    �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  �@��TextAlignment$ Right=���  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �������  � ��
 ���� ��  � |� .  �� " ��.���  � ��$ font\YoonYGO550 TT.ttf)���  � ��$
 15pt(���  � ��$
 Bold"���  � ��
 �   ���  � ��
 �������  �@��TextAlignment$ Right=���  � |� .  �� " ��.���  � �� �� "	 ��.R��� 

 ����  � .+��� $ mcoff��$-��'image\12_Manual 동작화면\MC_OFF.bmp���� $
 mcon��$,��&image\12_Manual 동작화면\MC_ON.bmp��._���  P�  �@d�	IsChecked$ falseY�. ��� $
 mcon�� � y�$ 	Collapsed=�.���� -   $	��800��$	��480��.+��� $9��3image\12_Manual 동작화면\수동설정배경.gif��$	��800���� $3��-image\12_Manual 동작화면\수정설정.gif��$��63.346�� �� Left$	 302��
 �� Top$ 7��$��46.676���� #��[  $��30(�$	 352��$ 20��$��36.674��$��120.024���� $�수동제어���� -   $ closeWindow��$6��0image\12_Manual 동작화면\되돌아가기.gif��$
��Fill=�$	 737��$ 14��$��40��$��50����  K� ZIndex$ -1��$��11.666��$��51�� J� RenderTransformOrigin$ 0.5,0.5&�$	 163��$	 251������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$	 Red�� � Offset$ 0Q�� $	 Red��$ 1Q�� $ White��$ 0.522Q�z�V� .~��� �� ��  �� Angle$	 360Q�T� �� -   $��button1��$ 12��$	 203��$��[BMS] MC_ON��$	��111��$	��148���� -   $��button2��$	 220��$	 203��$��[DSP] TsReady��$	��111��$	��148���� $	 370��$	 253��$��11.666��$ -1��$ 0.5,0.5&�$��51������ $ 0.5,1&�$ 0.5,0&�.��� $	 Red��$ 0Q�� $	 Red��$ 1Q�� $ White��$ 0.522Q�z�V� .~��� �� �� $	 360Q�T� �� -   $��button4��$	 625��$	 138��$��TsBatteryStart��$	��111��$	��148���� $	 573��$	 253��$��11.666��$ -1��$ 0.5,0.5&�$��51������ $ 0.5,1&�$ 0.5,0&�.��� $	 Red��$ 0Q�� $	 Red��$ 1Q�� $ White��$ 0.522Q�z�V� .~��� �� �� $	 360Q�T� �� -   $��button5��$	 625��$	 280��$��TsBatteryFinish��$	��111��$	��148��