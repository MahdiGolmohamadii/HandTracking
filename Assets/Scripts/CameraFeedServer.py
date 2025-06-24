import cv2
import mediapipe as mp
import math
import socket


width, height = 1280, 720

cap = cv2.VideoCapture(1)
cap.set(cv2.CAP_PROP_FRAME_WIDTH,width)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT,height)

mp_drawings = mp.solutions.drawing_utils

mp_hand = mp.solutions.hands
hand = mp_hand.Hands()

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddPort = ("127.0.0.1", 5052)

while True:
    success, img = cap.read()
    if success:
        RGB_img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        resualt = hand.process(img)
        data = []
        if resualt.multi_hand_landmarks:
            for hand_landmark in resualt.multi_hand_landmarks:
                # print(hand_landmark)
                mp_drawings.draw_landmarks(img, hand_landmark, mp_hand.HAND_CONNECTIONS)
            
            
            landmarks = resualt.multi_hand_landmarks[0]
            for landmark in landmarks.landmark:
                data.extend([landmark.x * width, height -  (landmark.y * height),landmark.z * width])

            # print(data)
            sock.sendto(str.encode(str(data)),  serverAddPort)
            
            
        img = cv2.resize(img,(0,0), None, 0.25,0.25)
        img = cv2.flip(img, 1)
        cv2.imshow("video feed", img)
        if cv2.waitKey(1) == ord('q'):
            break

cap.release()
cv2.destroyAllWindows() 