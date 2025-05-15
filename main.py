import requests
import datetime
import pytz
import time

API_KEY = "c511651c1c924ddc9b621f261b3ee649"
# GET 요청을 보낼 URL
url = f'https://openexchangerates.org/api/latest.json?app_id={API_KEY}&base=USD&prettyprint=true&show_alternative=false'  # 실제 API 엔드포인트로 변경해야 합니다.
headers = {
    'Content-Type': 'application/json'  # 요청에 따라 필요할 수 있습니다.
}
try:
    # GET 요청 보내기
    response = requests.get(url)

    # 요청이 성공했는지 확인 (상태 코드 200 OK)
    if response.status_code == 200:
        data = response.json()
        timestamp = data["timestamp"]
        rates = data["rates"]
        KRW = rates["KRW"]
        
        # 특정 시간대 지정 (Asia/Seoul - 한국 표준시)
        korea_timezone = pytz.timezone('Asia/Seoul')
        datetime_korea = datetime.datetime.fromtimestamp(timestamp, korea_timezone)
        
        print(f"한국 시간 (Asia/Seoul): {datetime_korea}")
        print(f"1USD -> {KRW}WON")
    else:
        print(f"GET 요청 실패. 상태 코드: {response.status_code}")
        print("에러 내용:", response.text)

except requests.exceptions.RequestException as e:
    print(f"요청 중 오류 발생: {e}")