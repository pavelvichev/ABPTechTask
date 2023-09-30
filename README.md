# AB Тестування API

## Вступ

Цей проект спрямований на надання можливості проводити A/B тестування для  додатків;

## Основні функціональність

- **Призначення користувачам експериментів**: Користувачам будуть призначатися експерименти на основі їхніх device-token. Якщо користувач вже призначений до якогось експерименту, він завжди отримує той же експеримент.

- **Збереження результатів**: Результати експериментів зберігаються в MS SQL базі даних для подальшого аналізу.

- **Статистика**: Ви можете перевірити статистику експериментів через веб-інтерфейс або отримати JSON-звіти.

## Технології та бібліотеки

- .NET: Ми використовуємо .NET для створення серверної частини додатку.

- MS SQL: База даних MS SQL використовується для збереження інформації про експерименти та їх результати.

## Структура БД та Збережені процедури

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/d0b2abec-6f0c-41d0-ad65-602680a17a94)

### Процедура для пошуку Експерименту за ключем 

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/cf86bc29-778b-46c1-b094-f914e51cbbd0)

### Процедура для пошуку Результату за девайс токеном

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/00611b83-b162-474a-84d0-11a41e00531f)

### Процедура для додавння Результату з девайс токеном

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/a01c6f84-b970-4365-a5e7-47d8a3380263)

### Процедури для виводу JSON статистики для  експериментів в залежності від ключа

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/79b4bb3a-cbbb-490a-831f-61cae3de3efc)

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/4c1b44d1-ed70-477d-940e-167b6777bd0c)

### Процедури для отримання звгальної кількості результатів(девайсів)

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/12a3f778-9995-4598-890c-67d96fa6acf2)

### Діагарма БД

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/6ac68c06-cc1d-4d1d-bfc0-b0f43141030d)

# Результати виконання роботи програми 

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/53cd18d1-c142-47b5-91c6-c19567e1f637)

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/19d491fc-c722-46e7-a21c-f86086044d8a)

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/e0cd6781-ea37-4b49-9de7-378c951982b3)

![image](https://github.com/pavelvichev/ABPTechTask/assets/71034124/3589553c-4305-4425-8982-395d8902a6ac)













