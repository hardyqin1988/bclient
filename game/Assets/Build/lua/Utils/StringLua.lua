
function string.IndexOf(a,b)
    local s = string.find(a,b);
    if s ~= nil then
        return s;
    end
    return -1;
end

function string.getCurrentTime()
    return os.date("%c");
end;

function string.getHttpHead(str,b)
    if str == nil or str == "" then return str end;
    if(b==true) then
        if(string.IndexOf(str,"https://")>-1) then
            return str;
        end
        if(string.IndexOf(str,"http://")>-1) then
            str = string.gsub(str,"http://","https://");
        end
        return str;
    end
    return str
end

function string.IsNullOrEmpty(a)
    if a == nil then
        return true;
    end
    if(a == "null") then
        return true;
    end;
    if(a == "") then
        return true;
    end
    if(a == " ") then
        return true;
    end;
    if(a == "NaN") then
        return true;
    end
    return false;
end

function string.Remove(a,b)
    return string.replace(a,b,"")
end;

function string.replace(a,b,c)
    return string.gsub(a, b, c);
end;

function string.Join(...)
    local arr = {}
    for i, a in ipairs({...}) do
        arr[#arr + 1] = tostring(a)
    end
    return table.concat(arr, "|");
end

function string.JoinTable(tab,a)
    return table.concat(tab, a);
end;

--[[--

Convert special characters to HTML entities.

The translations performed are:

-   '&' (ampersand) becomes '&amp;'
-   '"' (double quote) becomes '&quot;'
-   "'" (single quote) becomes '&#039;'
-   '<' (less than) becomes '&lt;'
-   '>' (greater than) becomes '&gt;'

@param string input
@return string

]]
function string.htmlspecialchars(input)
    for k, v in pairs(string._htmlspecialchars_set) do
        input = string.gsub(input, k, v)
    end
    return input
end
string._htmlspecialchars_set = {}
string._htmlspecialchars_set["&"] = "&amp;"
string._htmlspecialchars_set["\""] = "&quot;"
string._htmlspecialchars_set["'"] = "&#039;"
string._htmlspecialchars_set["<"] = "&lt;"
string._htmlspecialchars_set[">"] = "&gt;"

--[[--

Inserts HTML line breaks before all newlines in a string.

Returns string with '<br />' inserted before all newlines (\n).

@param string input
@return string

]]
function string.nl2br(input)
    return string.gsub(input, "\n", "<br />")
end

--[[--

Returns a HTML entities formatted version of string.

@param string input
@return string

]]
function string.text2html(input)
    input = string.gsub(input, "\t", "    ")
    input = string.htmlspecialchars(input)
    input = string.gsub(input, " ", "&nbsp;")
    input = string.nl2br(input)
    return input
end

--[[--

Split a string by string.

@param string str
@param string delimiter
@return table

]]
function string.split(str, delimiter)
    if (delimiter=='') then return false end
    local pos,arr = 0, {}
    -- for each divider found
    for st,sp in function() return string.find(str, delimiter, pos, true) end do
        table.insert(arr, string.sub(str, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(str, pos))
    return arr
end

--Ã¿¸ö×Ö·û
function string.gsplit(str)
	local a = {};
    local b = StringUtils.splitStr(str);
    local l = b.Length;
    for i=1,l do
        a[i] = b[i-1];
    end;
    return a;
end

--½«×Ö·û´®ÖÐµÄ\nÖØÐÂ×éºÏ
function string.njoin_(str)
    local arr = string.gsplit(str);
    local info = "";
    local s = "";
    for i=1,table.getn(arr) do
        s = s .. arr[i];
        if s=="\\n" then
            info = info .. "\n";
            s="";
        else
            if s ~="\\" then
                info = info .. s;
                s="";
            end
        end
        
    end
    return info;
end
--½«×Ö·û´®ÖÐµÄ\nÖØÐÂ×éºÏ
function string.njoin(str)
    local arr = string.split(str,"\\n");
    local info = "";
    local count = table.getn(arr);
    for i=1, count do
        info= i==count and (info..arr[i]) or (info..arr[i].."\n");
    end
    return info;
end
--[[--

Strip whitespace (or other characters) from the beginning of a string.

@param string str
@return string

]]
function string.ltrim(str)
    return string.gsub(str, "^[ \t\n\r]+", "")
end

--[[--

Strip whitespace (or other characters) from the end of a string.

@param string str
@return string

]]
function string.rtrim(str)
    return string.gsub(str, "[ \t\n\r]+$", "")
end

--[[--

Strip whitespace (or other characters) from the beginning and end of a string.

@param string str
@return string

]]
function string.trim(str)
    str = string.gsub(str, "^[ \t\n\r]+", "")
    return string.gsub(str, "[ \t\n\r]+$", "")
end

--[[--

Make a string's first character uppercase.

@param string str
@return string

]]
function string.ucfirst(str)
    return string.upper(string.sub(str, 1, 1)) .. string.sub(str, 2)
end

--[[--

@param string str
@return string

]]
function string.urlencodeChar(char)
    return "%" .. string.format("%02X", string.byte(c))
end

--[[--

URL-encodes string.

@param string str
@return string

]]
function string.urlencode(str)
    -- convert line endings
    str = string.gsub(tostring(str), "\n", "\r\n")
    -- escape all characters but alphanumeric, '.' and '-'
    str = string.gsub(str, "([^%w%.%- ])", string.urlencodeChar)
    -- convert spaces to "+" symbols
    return string.gsub(str, " ", "+")
end

--[[--

Get UTF8 string length.

@param string str
@return int

]]
function string.utf8len(str)
    local len  = #str
    local left = len
    local cnt  = 0
    local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
    while left ~= 0 do
        local tmp = string.byte(str, -left)
        local i   = #arr
        while arr[i] do
            if tmp >= arr[i] then
                left = left - i
                break
            end
            i = i - 1
        end
        cnt = cnt + 1
    end
    return cnt
end

--[[--

Return formatted string with a comma (",") between every group of thousands.

**Usage:**

    local value = math.comma("232423.234") -- value = "232,423.234"


@param number num
@return string

]]
function string.formatNumberThousands(num)
    local formatted = tostring(tonumber(num))
    while true do
        formatted, k = string.gsub(formatted, "^(-?%d+)(%d%d%d)", '%1,%2')
        if k == 0 then break end
    end
    return formatted
end

function string.isString(a)
    local t = type(a);
    if(t == "string") then
        return true;
    end
    return false;
end;

--a µÄÀàÐÍ
function string.getType(a)
    return type(a);
end;

function string.numToString(a)
    return tostring(a);
end;

function string.stringToNum(a)
    return tonumber(a);
end;

function string.charAt(s,i)
    return string.sub(s,i,i);
end

function string.lowerChar(s)
    return string.lower(s);
end

