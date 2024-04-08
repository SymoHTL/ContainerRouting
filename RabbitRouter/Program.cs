

// das sind alle cities, jede city ist nur zur nächsten city verbunden

var cityGraph = new[] { Cities.Osaka,
  Cities.Kyoto,
  Cities.Tokyo,
  Cities.Busan,
  Cities.Shanghai,
  Cities.Hongkong,
  Cities.Peking,
  Cities.Moskau,
  Cities.StPetersburg,
  Cities.Hamburg,
  Cities.Rotterdam,
  Cities.Wien,
  Cities.Mailand,
  Cities.Rom,
  Cities.Turin,
  Cities.Paris,
  Cities.Barcelona,
  Cities.Lissabon,
  Cities.London
};


var nodes = new List<Node>();

for (var i = 0; i < cityGraph.Length; i++) {
  var current = cityGraph[i];
  var next = cityGraph[(i + 1) % cityGraph.Length];

  for (var j = 0; j < cityGraph.Length; j++) {
    // We skip if the 'end' city is the current city or the next city
    if (j != i && j != (i + 1) % cityGraph.Length) {
      var end = cityGraph[j];
      var node = new Node(current, next, end);
      nodes.Add(node);
    }
  }
}

Console.WriteLine(nodes.Count);
nodes.ForEach(Console.WriteLine);


