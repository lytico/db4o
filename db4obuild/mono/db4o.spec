#
# RPM spec file for db4o Mono build
#

Summary: A native OODBMS for Java/.NET/Mono - Mono version
Name: db4o
Version: @db4oversion@
Release: @db4obuild@
License: GPL
Group: Development
Source: http://www.db4o.com/community/db4o-@db4oversion@-mono.tar.gz
URL: http://www.db4o.com/
Distribution: db4o
Vendor: db4objects Inc.
Packager: db4o <info@db4o.com>

%description
Embed db4o's native Java or .NET/Mono open source object database
engine into your product and experience unparalleled ease-of-use.
db4o is simple to install, integrate, and deploy.

%prep
%setup
echo %{buildroot}
echo $RPM_BUILD_ROOT

%build
make buildall

%install
make install

%files
%defattr(-,root,root)
%doc doc/*
/usr/lib/db4o/*