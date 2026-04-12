#!/bin/bash

# Директории для обработки
dirs=(".")
output="result.txt"

# Очищаем результат
> "$output"

# Текущая директория
current_dir=$(pwd)

for dir in "${dirs[@]}"; do
    # Рекурсивно находим все .cs файлы, сортируем, исключаем Debug
    find "$dir" -type f -name "*.cs" -not -path "*/Debug/*" | sort | while read -r file; do
        # Путь относительно текущей директории
        relative_path=$(realpath --relative-to="$current_dir" "$file")
        # Пишем комментарий с разделителем
        echo "//${relative_path}:" >> "$output"
        echo "//----------------------------------------" >> "$output"
        # Добавляем содержимое файла, удаляя лишние пустые строки
        sed '/^[[:space:]]*$/d' "$file" >> "$output"
        # Пустая строка между файлами
        echo "" >> "$output"
    done
done

# Принудительно сохраняем как UTF-8
iconv -f UTF-8 -t UTF-8 "$output" -o "$output"
