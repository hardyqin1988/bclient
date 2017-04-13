require "Utils/StringLua"
local inspect = require "3rdParty/inspect"

local print_l = print

function traceback ()
	local stack = "stack traceback:\n"
	local level = 2
	while true do
		local info = debug.getinfo(level, "nSl")
		if not info then break end
		if info.what ~= "C" and info.short_src ~= "Utils/Logger" then
			local func = "";
			if info.name ~= nil then 
				func = string.format("'%s'", info.name)
			else 
				func = string.format("<%s:%d>", info.short_src, info.linedefined) 
			end
			stack = stack .. string.format("\t%s:%d in function %s\n", info.short_src, info.currentline, func)
		end
		level = level + 1
	end
	return stack
end


local function printstack(msg)
	local msg = msg or ""
	if UNITY_EDITOR then
		local stack = traceback()
		msg = msg .. "\n" .. stack
	end
	print_l(msg)
end

local function logmsg(s)
	local msg = tostring(s)
	if type(s) == "table" then
		msg = msg .. "\n" .. inspect(s)
	end
	return msg
end

local function applycolor(msg, clr)
	local ret = ""
	local lines = string.split(msg, "\n")
	for i=1, #lines do
		ret = ret .. string.format("<color=%s>%s</color>", clr, lines[i]) .. "\n"
	end
	return ret
end

function log (s)
	if not RELEASE then
		printstack(logmsg(s))
	end
end

function logFormat (...)
	if not RELEASE then
		printstack(string.format(...))
	end
end

-- color is #rrggbbaa
function logClr (clr, s)
	if not RELEASE then
		printstack(applycolor(logmsg(s), clr))
	end
end

function logFormatClr (clr, ...)
	if not RELEASE then
		printstack(applycolor(string.format(...), clr))
	end
end

function logRed (s) 
	logClr("#ff0000ff", s) 
end

function logYellow (s) 
	logClr("#ffff00ff", s) 
end

function logBlue (s) 
	logClr("#0000ffff", s) 
end

function logGreen (s) 
	logClr("#00ff00ff", s) 
end

function logFormatRed (...) 
	logFormatClr("#ff0000ff", ...) 
end

function logFormatYellow (...) 
	logFormatClr("#ffff00ff", ...) 
end

function logFormatBlue (...) 
	logFormatClr("#0000ffff", ...) 
end

function logFormatGreen (...) 
	logFormatClr("#00ff00ff", ...) 
end

print = log
