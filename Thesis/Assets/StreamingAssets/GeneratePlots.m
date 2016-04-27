thisFile = mfilename('fullpath');

parentDir = fileparts(thisFile);

dataDir   = [parentDir filesep 'Data'];
imgDir    = [parentDir filesep 'Images'];

if ~exist(imgDir, 'dir')
    mkdir(imgDir);
end

dataMatch = [dataDir filesep '*.csv'];
cd(dataDir);

dirData = dir(dataMatch)

[m, n] = size(dirData);

%for i = 1:m
for i = 1:m
    file = dirData(i).name;
    [p, name, ext] = fileparts(file);

    fullFile = strcat(dataDir, filesep, file);
    T = readtable(fullFile, 'Delimiter', ',');
    [numUsers, numCols] = size(T);
    table = T{:,{'Day0', 'Day1', 'Day2', 'Day3', 'Day4'}};
    labels = T{:,{'Label'}};
    h = figure('Name', name);
    title(name);
    hold on;
    for j = 1:numUsers
        X = 1:5;
        Y = table(j,1:5);
        plot(X,Y);
    end
    ax = gca;
    ax.XTick = 1:5;
    if strcmp(labels(1),'2D')
        legend(labels);
    end
    imageFile = strcat(imgDir, filesep, name);
    print(imageFile, '-dpng');
    hold off
    close(h);
end