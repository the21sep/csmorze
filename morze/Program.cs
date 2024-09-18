var alphabet = new Dictionary<char,string> () {
    ['A'] = ".-",
    ['B'] = "-...",
    ['C'] = "-.-.",
    ['D'] = "-..",
    ['E'] = ".",
    ['F'] = "..-.",
    ['G'] = "--.",
    ['H'] = "....",
    ['I'] = "..",
    ['J'] = ".---",
    ['K'] = "-.-",
    ['L'] = ".-..",
    ['M'] = "--",
    ['N'] = "-.",
    ['O'] = "---",
    ['P'] = ".--.",
    ['Q'] = "--.-",
    ['R'] = ".-.",
    ['S'] = "...",
    ['T'] = "-",
    ['U'] = "..-",
    ['V'] = "...-",
    ['W'] = ".--",
    ['X'] = "-..-",
    ['Y'] = "-.--",
    ['Z'] = "--..",
    ['0'] = "-----",
    ['1'] = ".----",
    ['2'] = "..---",
    ['3'] = "...--",
    ['4'] = "....-",
    ['5'] = ".....",
    ['6'] = "-....",
    ['7'] = "--...",
    ['8'] = "---..",
    ['9'] = "----."    
};

Console.WriteLine("Введите текст на английском для перевода в азбуку Морзе");
string text = Console.ReadLine();
string caps = text.ToUpper();
int dit = 50;
int dah = 3 * dit;
for (int i = 0; i < text.Length; i++) {
    char letter = caps[i];
    string code = alphabet.ContainsKey(letter) ? alphabet[letter] : "";
    Console.WriteLine(letter + " (" + code + ")");
    for (int j = 0; j < code.Length; j++) {
        Sound.Beep(1000, code[j] == '.' ? dit : dah);
        Thread.Sleep(dit);
    }
    Thread.Sleep(dah);
}
