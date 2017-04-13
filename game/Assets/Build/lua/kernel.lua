--kernel property
function ff_get_property_bool(pid, property_name)					
	KernelBridge.GetPropertyBool(pid, property_name)
end

function ff_get_property_byte(pid, property_name)					
	KernelBridge.GetPropertyByte(pid, property_name)
end

function ff_get_property_int(pid, property_name)					
	KernelBridge.GetPropertyInt(pid, property_name)
end

function ff_get_property_float(pid, property_name)					
	KernelBridge.GetPropertyFloat(pid, property_name)
end

function ff_get_property_long(pid, property_name)					
	KernelBridge.GetPropertyLong(pid, property_name)
end

function ff_get_property_string(pid, property_name)					
	KernelBridge.GetPropertyString(pid, property_name)
end

function ff_get_property_pid(pid, property_name)					
	KernelBridge.GetPropertyPid(pid, property_name)
end

function ff_get_property_bytes(pid, property_name)	
	KernelBridge.GetPropertyBytes(pid, property_name)
end
