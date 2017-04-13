require "Utils/Class"

function behaviour(classname, super)
	local c = InitNewBehaviour[classname];
    if c ~= nil then
        return c;
    end

	local cls = class(classname, super);
	InitNewBehaviour[classname] = cls;
    return cls
end

function behaviourSingleton(classname, super)
    local b = behaviour(classname, super)
    b.isSingleton = true;
    return b;
end

InitNewBehaviour = {};

function behaviourNew(classname)
    local cls = InitNewBehaviour[classname]
    if cls == nil then
        return nil;
    end
    local c = cls;
    if cls.isSingleton == nil or cls.isSingleton == false then
        c = InitNewBehaviour[classname].New();
    end
    c.classname = classname;
    return c;
end