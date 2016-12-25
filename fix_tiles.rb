dir = 'Assets'

files = []
files += Dir["#{dir}/**/*.unity"]
files += Dir["#{dir}/**/*.asset"]

files.each { |file|
  old_text = File.read file
  new_text = old_text.gsub 'e13: 0.59999996', 'e13: 0'
  next if old_text == new_text
  puts "Fixed: #{file}"
  File.open(file, 'w') { |f| f.write new_text }
}