import requests
import base64

API_BASE = "http://localhost:5010/api"


def convert_image(format, image_path, data={}):
    url = f"{API_BASE}/convert/{format}"
    with open(image_path, "rb") as img_file:
        files = {"image": img_file}
        return requests.post(url, files=files, data=data)


def convert_image_base64(format, image_path, data={}):
    url = f"{API_BASE}/convert/{format}"
    with open(image_path, "rb") as img_file:
        b64_string = base64.b64encode(img_file.read()).decode('utf-8')
    payload = data.copy()
    payload["image"] = b64_string
    return requests.post(url, json=payload)


def get_json(endpoint):
    url = f"{API_BASE}/{endpoint}"
    res = requests.get(url)
    res.raise_for_status()
    return res.json()
