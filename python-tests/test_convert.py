import pytest
from utils import convert_image, convert_image_base64


def test_conversion_form():
    for f in ["html", "ansi", "rtf", "txt", "emoji", "latex", "svg"]:
        res = convert_image(
            f,
            "small.jpg",
            {"scale": "10",
             "characters": "@%#*+=-:.",
             "platform": "Windows Console"}
        )
        assert res.status_code == 200
        res_json = res.json()
        assert "content" in res_json
        assert len(res_json["content"]) > 0

        res = convert_image(
            f,
            "big.jpg",
            {"scale": "10",
             "characters": "@%#*+=-:.",
             "platform": "Windows Console"}
        )
        assert res.status_code == 200
        res_json = res.json()
        assert "content" in res_json
        assert len(res_json["content"]) > 0


def test_conversion_json():
    for f in ["html", "ansi", "rtf", "txt", "emoji", "latex", "svg"]:
        res = convert_image_base64(
            f,
            "small.jpg",
            {"scale": "10",
             "characters": "@%#*+=-:.",
             "platform": "Windows Console"}
        )
        assert res.status_code == 200
        res_json = res.json()
        assert "content" in res_json
        assert len(res_json["content"]) > 0

        res = convert_image_base64(
            f,
            "big.jpg",
            {"scale": "10",
             "characters": "@%#*+=-:.",
             "platform": "Windows Console"}
        )
        assert res.status_code == 200
        res_json = res.json()
        assert "content" in res_json
        assert len(res_json["content"]) > 0
