function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

--Create an class.
function class(classname, super)
    super = InitNewClasses[super];
    local superType = type(super)
    local cls

    if superType ~= "function" and superType ~= "table" then
        superType = nil
        super = nil
    end

    if superType == "function" or (super and super.__ctype == 1) then
        -- inherited from native C++ Object
        cls = {}

        if superType == "table" then
            -- copy fields from super
            for k,v in pairs(super) do cls[k] = v end
            cls.__create = super.__create
            cls.super    = super
        else
            cls.__create = super
        end

        cls.ctor    = function() end
        cls.__cname = classname
        cls.__ctype = 1

        function cls.New(...)
            local instance = cls.__create(...)
            -- copy fields from class to native object
            for k,v in pairs(cls) do instance[k] = v end
            instance.class = cls
            instance:ctor(...)
            return instance
        end

    else
        -- inherited from Lua Object
        if super then
            cls = clone(super)
            cls.super = super
        else
            cls = {ctor = function() end}
        end

        cls.__cname = classname
        cls.__ctype = 2 -- lua
        cls.__index = cls

        function cls.New(...)
            local instance = setmetatable({}, cls)
            instance.class = cls
            instance:ctor(...)
            return instance
        end
    end

    InitNewClasses[classname] = cls;

    return cls
end

InitNewClasses = {};
ProtoClasses_list = {};

function new(_className)
    if InitNewClasses[_className] == nil then
        logRed("Error: class is not reg!" .. _className);
        return;
    end

    local c = InitNewClasses[_className].New();
    c.classname = _className;
    return c;
end

RegProtoClasses = {};

function regProtoClasses(_className,_class)
    RegProtoClasses[_className] = _class;
    table.insert(ProtoClasses_list,_className);
end

function newProto(_className)
    if RegProtoClasses[_className] == nil then
        logRed("Error: protoClass is not reg!" .. _className);
        return;
    end

    local c = RegProtoClasses[_className].New();
    c.classname = _className;
    return c;
end

