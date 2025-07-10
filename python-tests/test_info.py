import re
from utils import get_json


def test_colors():
    colors = get_json("colors")
    assert "Black" in colors


def test_html_fonts():
    fonts = get_json("fonts/html")
    assert any("monospace" in f for f in fonts["html"])


def test_fonts():
    fonts = get_json("fonts")
    assert isinstance(fonts, dict)
    assert any("rtf" in f for f in fonts.keys())
